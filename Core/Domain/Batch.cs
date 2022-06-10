namespace Core.Domain
{
    public class Batch : EntityBase
    {
        
        public DateTimeOffset? UploadTime { get; set; }
        public double? PurchasePrice { get; set; }
        public string? UnityOfMeasurement { get; set; }
        public double? Value { get; set; }
        public int? DiscountTypeId { get; set; }
        public int? PlayerId { get; set; }
        public ICollection<DiscountCode>? DiscountCodes { get; set; }
        public DiscountType? DiscountType { get; set; }
        public Player? Player { get; set; }
    }
}
