using Core.Domain;
using Enum;

namespace DTOs.BodyDtos
{
    public class UploadCsvBodyDto
    {
        public int? PlayerId { get; set; }
        public DiscountTypeBodyDto? DiscountType { get; set; }
    }
}
