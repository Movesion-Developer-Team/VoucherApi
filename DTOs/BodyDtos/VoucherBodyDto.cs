namespace DTOs.BodyDtos
{
    public class VoucherBodyDto : BaseBody
    {
        public string? Name { get; set; }
        public int CategoryId { get; set; }
        public int DiscountId { get; set; }
    }
}
