namespace Core.Domain
{
    public class DiscountCode : EntityBase
    {
        public string? Code { get; set; }
        public int? DiscountId { get; set; }
        public int? UsageLimit { get; set; }
        public bool? IsAssignedToUser { get; set; }
        public bool? IsAssignedToCompany { get; set; }
        public Discount? Discount { get; set; }
        public ICollection<Company>? Companies { get; set; }
        public ICollection<Purchase>? Purchases { get; set; }
        public ICollection<CompanyDiscountCode>? CompanyDiscountCodes { get; set; }

    }
}
