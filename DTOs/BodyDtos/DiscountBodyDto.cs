namespace DTOs.BodyDtos
{
    public class DiscountBodyDto : BaseBody
    {
        public string? Name { get; set; }
        public string? LinkTermsAndConditions { get; set; }
        public string? UnityOfMeasurement { get; set; }
        public float? DiscountValue { get; set; }
        public int? InitialPrice { get; set; }
        public int? FinalPrice { get; set; }
        public int? DiscountTypeId { get; set; }
        public int? PlayerId { get; set; }
    }
}
