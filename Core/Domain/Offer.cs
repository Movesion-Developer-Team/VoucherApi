namespace Core.Domain
{
    public class Offer : EntityBase
    {
        public double? Price { get; set; }
        public int? Availability { get; set; }
        public int? CompanyId { get; set; }
        public int? DiscountCodeId { get; set; }
        public Company? Company { get; set; }
        public DiscountCode? DiscountCode { get; set; }
    }
}
