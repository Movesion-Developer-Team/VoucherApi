namespace Core.Domain
{
    public class DiscountCode : EntityBase
    {
        public string? Code { get; set; }
        public int? BatchId { get; set; }
        public int? UsageLimit { get; set; }
        public bool? IsAssignedToUser { get; set; }
        public int? DiscountId { get; set; }
        public int? UserId { get; set; }
        public bool? TemporaryReserved { get; set; }
        public DateTimeOffset? ReservationTime { get; set; }
        public Batch? Batch { get; set; }
        public User? User { get; set; }
        public ICollection<Purchase>? Purchases { get; set; }
        public Discount? Discount { get; set; }

    }
}
