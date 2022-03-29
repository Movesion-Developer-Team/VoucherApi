namespace Core.Domain;

public class Agency : EntityBase
{
    public string Name { get; set; }
    public DateTime ContactDate { get; set; }
    public ICollection<Category> Categories { get; set; }
    public ICollection<Player> Players { get; set; }
    public List<AgencyCategory> AgencyCategories { get; set; }
    public List<AgencyPlayer> AgencyPlayers { get; set; }

}