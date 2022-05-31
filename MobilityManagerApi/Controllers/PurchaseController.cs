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

        //[Authorize]
        //[HttpPost]
        //[ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]

        //public async Task<IActionResult> CountPurchaseAmount([FromBody] DiscountBodyDto body)
        //{
        //    var response = new BaseResponse();
        //    var discountType = await _unitOfWork.Discount.FindDiscountType(body.DiscountTypeId);
        //    if (discountType.Name != DiscountTypes.SingleUse.ToString())
        //    {
        //        response.Message = "Currently only Single Use Discount purchases are implemented inside the system";
        //        return BadRequest(response);
        //    }

            
        //}

    }
}
