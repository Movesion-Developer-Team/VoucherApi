using Enum;

namespace DTOs.BodyDtos
{
    public class UploadCsvBodyDto
    {
        public string? FilePath { get; set; }
        public int? PlayerId { get; set; }
        public DiscountType DiscountType { get; set; }
    }
}
