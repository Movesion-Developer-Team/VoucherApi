namespace Core.Domain
{
    public class Voucher : EntityBase
    {
        public string Name { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public Discount Discount { get; set; }
        public int DiscountId { get; set; }

    }
}
