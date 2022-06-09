

namespace Core.Domain;

public class Company : EntityBase
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public DateTimeOffset ContactDate { get; set; }
    public ICollection<CompanyPortfolio>? CompanyPortfolios { get; set; }
    public ICollection<Discount>? Discounts { get; set; }
    public ICollection<Player>? Players { get; set; }
    public ICollection<User>? Users { get; set; }
    public ICollection<InvitationCode>? InvitationCodes { get; set; }
    public ICollection<CompanyPlayer>? CompanyPlayers { get; set; }


}