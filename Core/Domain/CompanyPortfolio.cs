namespace Core.Domain
{
    public class CompanyPortfolio : EntityBase
    {
        public int? CompanyId { get; set; }
        public int? DiscountId { get; set; }
        public Company? Company { get; set; }
        public Discount? Discount { get; set; }
        public ICollection<DiscountCode>? DiscountCodes { get; set; }
    }
}
