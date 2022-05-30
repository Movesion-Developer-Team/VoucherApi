using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.StripeModels;

namespace MobilityManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]/")]
    [EnableCors]
    public class StripeController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Create(PaymentIntentCreateRequest request)
        {
            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = await paymentIntentService.CreateAsync(new PaymentIntentCreateOptions()
                {
                    Amount = 1,
                    Currency = "eur",
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions()
                    {
                        Enabled = true
                    }
                }
            );
            return Ok(new {clientSecret = paymentIntent.ClientSecret});
        }
        
    }

}