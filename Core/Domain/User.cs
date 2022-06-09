namespace Core.Domain
{
    public class User : EntityBase
    {
        public string? IdentityUserId { get; set; }
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }
        public ICollection<DiscountCode>? DiscountCodes { get; set; }
        public JoinRequest? JoinRequest { get; set; }
        public ICollection<Purchase>? Purchases { get; set; }
    }
}
