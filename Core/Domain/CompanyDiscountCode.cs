namespace Core.Domain
{
    public class CompanyDiscountCode
    {
        public int? CompanyId { get; set; }
        public int? DiscountCodeId  { get; set; }
        public Company? Company { get; set; }
        public DiscountCode? DiscountCode { get; set; }
    }
}
