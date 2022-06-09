namespace Core.Domain
{
    public class DiscountType : EntityBase
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public ICollection<Player>? Players { get; set; }
        public ICollection<Discount>? Discounts { get; set; }
        public ICollection<Batch>? Batches { get; set; }
        public ICollection<PlayerDiscountType>? PlayerDiscountTypes { get; set; }
    }
}
