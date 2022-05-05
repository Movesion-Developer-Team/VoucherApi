

namespace Core.Domain;

public class Company : EntityBase
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public int? NumberOfEmployees { get; set; }
    public DateTime ContactDate { get; set; }
    public List<User>? Workers { get; set; }
    public ICollection<Player>? Players { get; set; }
    public List<CompanyPlayer>? CompanyPlayers { get; set; }

}