namespace Core.Domain
{
    public class DiscountCode : EntityBase
    {
        public string? Code { get; set; }
        public int? DiscountId { get; set; }
        public Voucher? Voucher { get; set; }
        public Discount? Discount { get; set; }
        
    }
}
