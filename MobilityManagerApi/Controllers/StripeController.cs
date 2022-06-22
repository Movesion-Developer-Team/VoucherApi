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
        private readonly IConfiguration _configuration;
        private readonly string discountIdKey = "discountId";
        private readonly string numberOfCodesKey = "numberOfCodesKey";
        private readonly string userIdKey = "userIdKey";

        public StripeController(IMapper mapper, VoucherContext vContext, IConfiguration configuration) : base(mapper, vContext)
        {
            _configuration = configuration;
        }

        [Authorize]
        [HttpPost]
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
                        {"CodiceFiscale",""},
                        {discountIdKey, discountId.ToString()},
                        {numberOfCodesKey, numberOfCodes.ToString()},
                        {userIdKey, user.Id.ToString()}
                    }
                }
            );
            
            response.Message = "Selected";
            response.ClientSecret = paymentIntent.ClientSecret;
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> WebHook()
        {
            var response = new BaseResponse();
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            string endpointSecret = _configuration.GetValue<string>("StripeEndPointSecret");
            try
            {
                var signatureHeader = Request.Headers["Stripe-Signature"];

                var stripeEvent = EventUtility.ConstructEvent(json,
                    signatureHeader, endpointSecret);
                response.Message = stripeEvent.Type;
                

                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    var discountId =
                        Int32.Parse(paymentIntent.Metadata.FirstOrDefault(cd => cd.Key == discountIdKey).Value);
                    var numberOfCodes =
                        Int32.Parse(paymentIntent.Metadata.FirstOrDefault(cd => cd.Key == numberOfCodesKey).Value);
                    var userId = Int32.Parse(paymentIntent.Metadata.FirstOrDefault(cd => cd.Key == userIdKey).Value);

                    await _unitOfWork.Discount.CompleteReservation(discountId, userId, numberOfCodes);
                    await _unitOfWork.Complete();
                    return Ok(response);
                }

                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed ||
                    stripeEvent.Type == Events.PaymentIntentCanceled)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    var discountId =
                        Int32.Parse(paymentIntent.Metadata.FirstOrDefault(cd => cd.Key == discountIdKey).Value);
                    var numberOfCodes =
                        Int32.Parse(paymentIntent.Metadata.FirstOrDefault(cd => cd.Key == numberOfCodesKey).Value);
                    var userId = Int32.Parse(paymentIntent.Metadata.FirstOrDefault(cd => cd.Key == userIdKey).Value);

                    await _unitOfWork.Discount.DeclineReservation(discountId, userId);
                    await _unitOfWork.Complete();
                    return BadRequest(response);
                }

                response.Message = $"Unhandled event: {stripeEvent.Type}";
                return BadRequest(response);

            }
            catch (StripeException ex)
            {
                response.Message = $"Unhandled event: {ex.Message}";
                return BadRequest(response);
            }
            catch (ArgumentNullException ex)
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
                response.Message = $"Internal server error: {ex.Message}";
                return BadRequest(response);
            }

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