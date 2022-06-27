namespace DTOs.BodyDtos
{
    public class UserDiscountCodeBodyDto
    {
        public string? OrderDateTime { get; set; }
        public string? PlayerName { get; set; }
        public long? FinalPrice { get; set; }
        public string? DiscountTypeName { get; set; }
        public int? PriceInPoints { get; set; }
        public string? Code { get; set; }
    }
}
