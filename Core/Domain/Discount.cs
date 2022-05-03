using Enum;

namespace Core.Domain
{
    public class Discount : EntityBase
    {

        public DiscountCode? DiscountCode { get; set; }
        public int? DiscountCodeId { get; set; }
        public int? PlayerId { get; set; }
        public string? LinkTermsAndConditions { get; set; }
        public string? UnityOfMeasurement { get; set; }

        public readonly List<int>? UsageTimes = new List<int>();
        public float? DiscountValue { get; set; }
        public int? NumberOfUsagePerCompany { get; set; }
        public int? NumberOfUsagePerUser { get; set; }
        public int? InitialPrice { get; set; }
        public int? FinalPrice { get; set; }


        public DiscountType DiscountType { get; set; }
        public Player Player { get; set; }
        public ValidityPeriod ValidityPeriod { get; set; }
        public ICollection<Voucher> Vouchers { get; set; }

    }
}
