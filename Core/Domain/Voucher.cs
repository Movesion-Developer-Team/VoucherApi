namespace Core.Domain
{
    public class Voucher : EntityBase
    {
        public string Name { get; set; }
        public int? DiscountCodeId { get; set; }
        public DiscountCode? DiscountCode { get; set; }

    }
}
