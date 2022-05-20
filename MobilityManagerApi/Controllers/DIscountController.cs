using System.Globalization;
using System.Reflection.Metadata;
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
using Extensions;

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
        [ProducesResponseType(typeof(CreateNewEntityResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateNewEntityResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNewDiscount([FromBody] DiscountBodyDto body)
        {
            var response = new CreateNewEntityResponseDto();
            var discount = _mapper.Map<Discount>(body);

            var checks = new List<int?>
            
            {
                body.PlayerId,
                body.DiscountTypeId
            };
            var namesOfCheck = new List<string>
            {
                nameof(body.PlayerId),
                nameof(body.DiscountTypeId)
            };
            for (int i = 0; i < checks.Count; i++)
            {
                if (checks[i] == null)
                {
                    response.Message = $"Please provide value of {namesOfCheck[i]}";
                    return BadRequest(response);
                }
            }

            try
            {
                var player = await _unitOfWork.Player.Find(p => p.Id == body.PlayerId).Include(p=>p.DiscountsTypes).SingleOrDefaultAsync();
                player.CheckForNull();
                var discountType = await _unitOfWork.Discount.FindDiscountType(body.DiscountTypeId);
                discountType.CheckForNull();
                if (player.DiscountsTypes != null && !player.DiscountsTypes.Contains(discountType))
                {
                    player.DiscountsTypes.Add(discountType);
                }

                await _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
            
            
            try
            {
                response.Id = await _unitOfWork.Discount.AddAsync(discount);
                await _unitOfWork.Complete();
                response.Message = "Done";
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [AuthorizeRoles(Role.SuperAdmin, Role.Admin, Role.User)]
        [HttpPost]
        [ProducesResponseType(typeof(GetAllDiscountsForPlayerResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllDiscountsForPlayerResponseDto), StatusCodes.Status400BadRequest)]
        [Route("{playerId}")]
        public async Task<IActionResult> GetAllDiscountsForPlayer([FromRoute] int playerId)
        {
            var response = new GetAllDiscountsForPlayerResponseDto();
            try
            {
                var discounts = await _unitOfWork.Discount.GetAllDiscountsForPlayer(playerId);
                response.Discounts = _mapper.ProjectTo<DiscountBodyDto>(discounts);
                discounts.CheckQueryForNull();
                response.Message = "Done";
                response.StatusCode = StatusCodes.Status200OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }


        }


        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
        [HttpDelete]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteDiscount([FromBody] DeleteDiscountBodyDto body)
        {
            var response = new BaseResponse();
            try
            {
                var discount = await _unitOfWork.Discount.Find(d => d.Id == body.DiscountId).FirstAsync();
                discount.CheckForNull();
                await _unitOfWork.Discount.RemoveAsync(body.DiscountId);
                await _unitOfWork.Complete();
                response.Message = "Deleted";
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }

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

            var discount = await _unitOfWork.Discount.Find(d => d.Id == discountId).Include(d=>d.DiscountCodes).SingleOrDefaultAsync();
            if (discount == null)
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

                    _unitOfWork.Discount.Update(discount);
                    if (discount.DiscountCodes == null)
                    {
                        discount.DiscountCodes = new List<DiscountCode>().AsQueryable();
                    };
                    discount.DiscountCodes = discount.DiscountCodes.Concat(codes);
                    await _unitOfWork.Complete();

                    response.Message = "Discounts are assigned to the player!";

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

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignDiscountCodesToCompany([FromBody] AssignDiscountCodesToCompanyBodyDto body)
        {
            var response = new BaseResponse();
            try
            {
                await _unitOfWork.Discount.AssignDiscountCodesToCompany(body.DiscountId, body.CompanyId,
                    body.NumberOfDiscounts);
                await _unitOfWork.Complete();
                response.Message = "Done";
                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);

            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
            catch (InvalidOperationException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Unexpected server error : {ex.Message}";
                return BadRequest(response);
            }
        }

    }
}
