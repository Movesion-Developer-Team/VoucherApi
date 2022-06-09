namespace DTOs.BodyDtos
{
    public class BatchBodyDto : BaseBody
    {
        public DateTimeOffset? UploadTime { get; set; }
        public double? PurchasePrice { get; set; }
        public string? UnityOfMeasurement { get; set; }
        public double? Value { get; set; }
        public int? DiscountTypeId { get; set; }
    }
}
