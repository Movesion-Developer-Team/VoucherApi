using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stripe.StripeModels
{
    public class Item
    {
        [JsonProperty("id")]
        public string Id { get; set; }

    }
}
