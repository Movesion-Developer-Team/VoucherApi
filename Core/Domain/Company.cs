namespace Core.Domain;

public class Company : EntityBase
{
    public string Name { get; set; }
    public DateTime ContactDate { get; set; }
    public ICollection<Category> Categories { get; set; }
    public ICollection<Player> Players { get; set; }
    public List<CompanyCategory> CompanyCategories { get; set; }
    public List<CompanyPlayer> CompanyPlayers { get; set; }

}