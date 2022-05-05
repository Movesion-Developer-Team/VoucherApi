using Enum;

namespace DTOs.BodyDtos
{
    public class UploadCsvBodyDto
    {
        public int? PlayerId { get; set; }
        public DiscountType DiscountType { get; set; }
    }
}
