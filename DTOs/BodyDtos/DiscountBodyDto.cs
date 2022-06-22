using Enum;

namespace DTOs.BodyDtos
{
    public class DiscountBodyDto : BaseBody
    {
        public string? Name { get; set; }
        public string? LinkTermsAndConditions { get; set; }
        public UnitiesOfMeasurement? UnityOfMeasurement { get; set; }
        public float? DiscountValue { get; set; }
        public long? InitialPrice { get; set; }
        public long? FinalPrice { get; set; }
        public int? DiscountTypeId { get; set; }
        public int? PlayerId { get; set; }
        public int? PriceInPoints { get; set; }
    }
}
