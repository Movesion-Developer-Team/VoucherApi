namespace Core.Domain
{
    public class Purchase : EntityBase
    {
        public DateTimeOffset? PurchaseTime { get; set; }
        public int? DiscountCodeId { get; set; }
        public int? UserId { get; set; }
        public DiscountCode? DiscountCode { get; set; }
        public User? User { get; set; }
    }
}
