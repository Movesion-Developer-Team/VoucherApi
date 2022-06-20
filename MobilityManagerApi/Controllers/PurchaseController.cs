using AutoMapper;
using DTOs.ResponseDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace BenefitsApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]/")]
    [EnableCors]
    public class PurchaseController : PadreController
    {

        public PurchaseController(IMapper mapper, VoucherContext vContext) : base(mapper, vContext)
        {
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]

        public async Task<IActionResult> ReserveCodes([FromQuery] int discountId, [FromQuery] int userId,
            [FromQuery] int numberOfDiscounts)
        {
            var response = new BaseResponse();
            try
            {
                await _unitOfWork.Discount.ReserveCodes(discountId, userId, numberOfDiscounts);
                await _unitOfWork.Complete();

                response.Message = "Reserved";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Unexpected server error: {ex.Message}";
                return BadRequest(response);
            }
           
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(GetTotalAmountResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetTotalAmountResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTotalAmount([FromQuery] int discountId, [FromQuery] int quantity)
        {
            var response = new GetTotalAmountResponseDto();
            try
            {
                response.TotalAmount = await _unitOfWork.Discount.OrderAmount(discountId, quantity);
                response.Message = "Total amount is provided";
                response.StatusCode = StatusCodes.Status200OK;
                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCodes.Status400BadRequest;
                return BadRequest(response);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCodes.Status400BadRequest;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Unexpected server error: {ex.Message}";
                response.StatusCode = StatusCodes.Status400BadRequest;
                return BadRequest(response);
            }
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CompleteOrder([FromQuery] string? status, [FromQuery] int? discountId, [FromQuery] int? numberOfCodes)
        {
            var response = new BaseResponse();
            var user = await GetCurrentUserInfo();
            if (status == "succeed")
            {
                try
                {
                    await _unitOfWork.Discount.CompleteReservation(discountId, (int)user.Id, (int)numberOfCodes);
                    await _unitOfWork.Complete();
                    response.Message = "Order completed";
                    return Ok(response);

                }
                catch (Exception ex)
                {
                    response.Message = $"Internal server error: {ex.Message}";
                    return BadRequest(response);
                }
            }
            else
            {
                response.Message = "Order cannot be completed";
                return BadRequest(response);
            }
        }

        //[Authorize]
        //[HttpGet]
        //[ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> GetAllOrdersOfCurrentUser()
        //{

        //}

    }
}
