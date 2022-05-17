using System.Globalization;
using AutoMapper;
using Core.Domain;
using CsvHelper;
using CsvHelper.Configuration;
using DTOs.BodyDtos;
using DTOs.MethodDto;
using DTOs.ResponseDtos;
using Enum;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using Persistence;
using UserStoreLogic;
using System.Text;

namespace MobilityManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [EnableCors]
    public class DiscountController : ControllerBase, IControllerBaseActions
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public DiscountController(IMapper mapper, VoucherContext vContext)
        {
            _mapper = mapper;
            _unitOfWork = new UnitOfWork(vContext);
        }

        

        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
        [HttpPost]
        [ProducesResponseType(typeof(CsvToDiscountCodesResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CsvToDiscountCodesResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CsvToDiscountCodes()
        {
            var response = new CsvToDiscountCodesResponseDto();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };
            
            try
            {
                Request.EnableBuffering();
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                if (file.Length > 0)
                {
                    
                    var stream = file.OpenReadStream();
                    var reader = new StreamReader(stream);
                    var csv = new CsvReader(reader, config);

                    response.UnassignedCollectionId = await
                        _unitOfWork.UnassignedDiscountCodeCollections.AddAsync(new UnassignedDiscountCodeCollection());

                    var codes = _mapper.ProjectTo<DiscountCode>(csv.GetRecords<CsvCodeDto>().AsQueryable()).ToList();

                    void Apply(DiscountCode code) => code.UnassignedCollectionId = response.UnassignedCollectionId;

                    foreach (var code in codes)
                    {
                        await Task.Run(() => Apply(code));
                    }

                    await _unitOfWork.DiscountCode.AddRangeAsync(codes);
                    await _unitOfWork.Complete();

                    response.Message = "Discounts are saved in Db without assignment to the player!";

                    return Ok(response);
                }

                response.Message = "Please provide a file";
                return BadRequest();
            }
            catch (Exception ex)
            {
                response.Message = $"Internal Server error: {ex.Message}";
                return BadRequest(response);
            }
        }

        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignCodesCollectionToPlayer([FromBody] AssignCodesCollectionToPlayerBodyDto body)
        {
            var response = new BaseResponse();
            IQueryable<DiscountCode> collection;
            if (body.UnassignedCollectionId == null)
            {
                response.Message = "Please provide Unassigned Code Collection id";
                return BadRequest(response);
            }
            try
            {
                collection = _unitOfWork.DiscountCode
                    .Find(dc => dc.UnassignedCollectionId == body.UnassignedCollectionId);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }

            List<Discount> newDiscountsList = new();
            void Apply(DiscountCode dc) => newDiscountsList.Add(new Discount
            {
                DiscountCodeId = dc.Id,
                PlayerId = body.PlayerId,
                DiscountType = _mapper.Map<DiscountType>(body.DiscountType),
                ValidityPeriod = new ValidityPeriod
                {
                    StartDate = body.StartDate,
                    EndDate = body.EndDate
                }
            });
            foreach (var c in collection)

            {
                await Task.Run(() => Apply(c));

            }

            await _unitOfWork.Discount.AddRangeAsync(newDiscountsList);
            await _unitOfWork.Complete();

            foreach (var c in collection)
            {
                _unitOfWork.DiscountCode.Update(c);
                c.UnassignedCollectionId = null;
            }
            await _unitOfWork.UnassignedDiscountCodeCollections.RemoveAsync((int)body.UnassignedCollectionId);
            await _unitOfWork.Complete();

            response.Message = "Discount codes assigned to a Player";
            return Ok(response);
        }

    }
}
