using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public class User : EntityBase
    {
        public string? IdentityUserId { get; set; }
        public int? CompanyId { get; set; }
        public string? PaymentCardTitle { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        public string? TelephoneNumber { get; set; }
        public string? Address { get; set; }
        public string? TaxCode { get; set; }
        public Company? Company { get; set; }
        public ICollection<DiscountCode>? DiscountCodes { get; set; }
        public JoinRequest? JoinRequest { get; set; }
        public ICollection<Purchase>? Purchases { get; set; }
    }
}
