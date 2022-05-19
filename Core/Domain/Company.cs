

namespace Core.Domain;

public class Company : EntityBase
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public int? NumberOfEmployees { get; set; }
    public DateTime ContactDate { get; set; }
    public ICollection<User>? Users { get; set; }
    public ICollection<DiscountCode>? DiscountCodes { get; set; }
    public ICollection<InvitationCode>? InvitationCodes { get; set; }
    public ICollection<CompanyDiscountCode>? CompanyDiscountCodes { get; set; }


}