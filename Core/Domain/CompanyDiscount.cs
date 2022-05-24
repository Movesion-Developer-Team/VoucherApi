namespace Core.Domain
{
    public class CompanyDiscount
    {
        public int? CompanyId { get; set; }
        public int? DiscountId { get; set; }
        public Company? Company { get; set; }
        public Discount? Discount { get; set; }
    }
}
