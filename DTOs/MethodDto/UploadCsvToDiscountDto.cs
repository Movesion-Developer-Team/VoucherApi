using Enum;

namespace DTOs.MethodDto
{
    public class UploadCsvToDiscountDto
    {
        public string? Code { get; set; }
        public int? PlayerId { get; set; }
        public DiscountType DiscountType { get; set; }

    }
}
