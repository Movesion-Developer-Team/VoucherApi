namespace DTOs
{
    public class CategoryDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<PlayerDto>? Players { get; set; }
        public List<VoucherDto>? Vouchers { get; set; }
        public List<AgencyDto>? Agencies { get; set; }
    }
}
