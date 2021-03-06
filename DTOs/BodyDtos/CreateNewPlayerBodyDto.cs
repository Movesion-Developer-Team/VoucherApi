using System.Drawing;

namespace DTOs.BodyDtos
{
    public class CreateNewPlayerBodyDto
    {
        public string? ShortName { get; set; }
        public string? FullName { get; set; }
        public int? CategoryId { get; set; }
        public int? DiscountTypeId { get; set; }
        public string? PlayStoreLink { get; set; }
        public string? AppStoreLink { get; set; }
        public string? LinkDescription { get; set; }
        public string? Color { get; set; }
    }
}
