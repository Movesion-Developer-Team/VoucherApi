using System.Drawing;

namespace DTOs
{
    public class PlayerDto
    {

        public string ShortName { get; set; }
        public string? FullName { get; set; }
        public string? PlayStoreLink { get; set; }
        public string? AppStoreLink { get; set; }
        public string? LinkDescription { get; set; }
        public KnownColor Color { get; set; }
        public int CategoryId { get; set; }
        public List<DiscountDto>? Discounts { get; set; } 
        public List<LocationDto>? Locations { get; set; }
        public List<PlayerContactDto>? PlayerContacts { get; set; }
        public List<CompanyDto>? Companies { get; set; }


    }
}
