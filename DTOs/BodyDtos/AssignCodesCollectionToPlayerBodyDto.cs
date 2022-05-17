using Core.Domain;
using Enum;

namespace DTOs.BodyDtos
{
    public class AssignCodesCollectionToPlayerBodyDto
    {
        public int? UnassignedCollectionId { get; set; }
        public int? PlayerId { get; set; }
        public DiscountTypeBodyDto? DiscountType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
