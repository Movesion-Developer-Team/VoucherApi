namespace Core.Domain
{
    public class Batch : EntityBase
    {
        
        public DateTimeOffset? UploadTime { get; set; }
        public ICollection<DiscountCode>? DiscountCodes { get; set; }
    }
}
