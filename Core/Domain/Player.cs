using System.Drawing;

namespace Core.Domain
{
    public class Player : EntityBase
    {
        public string? ShortName { get; set; }
        public string? FullName { get; set; }
        public string? PlayStoreLink { get; set; }
        public string? AppStoreLink { get; set; }
        public string? LinkDescription { get; set; }
        public string? Color { get; set; }
        public BaseImage? Image { get; set; }
        
        public ICollection<CompanyPlayer>? CompanyPlayers { get; set; }
        public ICollection<Company>? Companies { get; set; }
        public ICollection<Category>? Categories { get; set; }
        public ICollection<Discount>? Discounts { get; set; }
        public ICollection<DiscountType>? DiscountsTypes { get; set; }
        public ICollection<Location>? Locations { get; set; }
        public List<PlayerLocation>? PlayerLocations { get; set; }
        public List<PlayerCategories>? PlayerCategories { get; set; }
        public ICollection<PlayerContact>? PlayerContacts { get; set; }
        public ICollection<PlayerDiscountType>? PlayerDiscountTypes { get; set; }
        public ICollection<Batch>? Batches { get; set; }

    }
}
