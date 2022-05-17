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
    [Route("[controller]/[action]/")]
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
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [Route("{discountId}")]
        public async Task<IActionResult> UploadCsv([FromRoute] int discountId)
        {
            var response = new BaseResponse();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };

            var discount = _unitOfWork.Discount.Find(d => d.Id == discountId);
            if (!await discount.AnyAsync())
            {
                response.Message = "Discount not found";
                return BadRequest(response);
            }
            
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

                    var codes = _mapper.ProjectTo<DiscountCode>(csv.GetRecords<CsvCodeDto>().AsQueryable()).ToList();

                    void Apply(DiscountCode code) => code.DiscountId = discountId;

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
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Unexpected server error: {ex.Message}";
                return BadRequest(response);
            }
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllDiscountTypesResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllDiscountTypesResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllDiscountTypes()
        {
            var response = new GetAllDiscountTypesResponseDto();
            try
            {
                var discountTypes = await _unitOfWork.Discount.GetAllDiscountTypes();
                response.DiscountTypes = _mapper.ProjectTo<DiscountTypeBodyDto>(discountTypes);
                response.Message = "Done";
                return Ok(response);

            }
            catch (ArgumentNullException ex)
            {
                response.Message = $"Internal server error: {ex.Message}";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Unexpected server error: {ex.Message}";
                return BadRequest((response));
            }
        }

    }
}
