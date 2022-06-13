using AutoMapper;
using DTOs.ResponseDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Stripe;

namespace MobilityManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]/")]
    [EnableCors]
    public class StripeController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public StripeController(IMapper mapper, VoucherContext vContext)
        {
            _mapper = mapper;
            _unitOfWork = new UnitOfWork(vContext);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(PaymentIntentResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PaymentIntentResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePaymentIntent([FromQuery] int discountId, [FromQuery] int numberOfCodes)
        {
            var response = new PaymentIntentResponseDto();
            var paymentIntentService = new PaymentIntentService();

            var amount = await _unitOfWork.Discount.OrderAmount(discountId, numberOfCodes);

            var paymentIntent = await paymentIntentService.CreateAsync(new PaymentIntentCreateOptions()
                {
                    Amount = amount,
                    Currency = "eur",
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions()
                    {
                        Enabled = true
                    }
                }
            );
            response.Message = "Selected";
            response.ClientSecret = paymentIntent.ClientSecret;
            return Ok(response);
        }

        

        //[Authorize]
        //[HttpPost]
        //[ProducesResponseType(typeof(PaymentIntentResponseDto), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(PaymentIntentResponseDto), StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult>AcceptOrDeclinePayment([FromQuery] )
        //{

        //}

    }

}