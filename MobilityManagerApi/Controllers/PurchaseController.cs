using System.Security.Policy;
using AutoMapper;
using DTOs.BodyDtos;
using DTOs.ResponseDtos;
using Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using UserStoreLogic;

namespace MobilityManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]/")]
    [EnableCors]
    public class PurchaseController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public PurchaseController(IMapper mapper, VoucherContext vContext)
        {
            _mapper = mapper;
            _unitOfWork = new UnitOfWork(vContext);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]

        public async Task<IActionResult> ReserveCodes([FromQuery] int discountId, [FromQuery] int userId,
            [FromQuery] int numberOfDiscounts)
        {
            var response = new BaseResponse();

            await _unitOfWork.Discount.ReserveCodes(discountId, userId, numberOfDiscounts);
            await _unitOfWork.Complete();

            response.Message = "Reserved";
            return Ok(response);
        }
    }
}
