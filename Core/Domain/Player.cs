using System.Drawing;

namespace Core.Domain
{
    public class Player : EntityBase
    {
        public string ShortName { get; set; }
        public string? FullName { get; set; }
        public string? PlayStoreLink { get; set; }
        public string? AppStoreLink { get; set; }
        public string? LinkDescription { get; set; }

        public KnownColor Color { get; set; }
        public Category? Category { get; set; }
        public ICollection<Discount>? Discounts { get; set; }
        public ICollection<Location>? Locations { get; set; }
        public List<PlayerLocation>? PlayerLocations { get; set; }
        public ICollection<PlayerContact>? PlayerContacts { get; set; }
        public ICollection<Company>? Companies { get; set; }
        public List<CompanyPlayer>? CompanyPlayers { get; set; }

    }
}
