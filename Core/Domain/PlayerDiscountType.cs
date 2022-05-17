namespace Core.Domain
{
    public class PlayerDiscountType
    {
        public int? PlayerId { get; set; }
        public int? DiscountTypeId { get; set; }
        public Player? Player { get; set; }
        public DiscountType? DiscountType { get; set; }
        
    }
}
