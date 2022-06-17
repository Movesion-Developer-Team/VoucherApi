using AutoMapper;
using BenefitsApi.Controllers;
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
    public class StripeController : PadreController
    {

        

        public StripeController(IMapper mapper, VoucherContext vContext) : base(mapper, vContext)
        {
            
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(PaymentIntentResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PaymentIntentResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePaymentIntent([FromQuery] int discountId, [FromQuery] int numberOfCodes)
        {
            var user = await GetCurrentUserInfo();
            var response = new PaymentIntentResponseDto();
            var paymentIntentService = new PaymentIntentService();
            await _unitOfWork.Discount.ReserveCodes(discountId, (int) user.Id, numberOfCodes);
            await _unitOfWork.Complete();

            var amount = await _unitOfWork.Discount.OrderAmount(discountId, numberOfCodes);

            var paymentIntent = await paymentIntentService.CreateAsync(new()
                {
                    Amount = amount*100,
                    Currency = "eur",
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions()
                    {
                        Enabled = true
                    },
                    Metadata = new Dictionary<string, string>
                    {
                        {"CodiceFiscale",""}
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