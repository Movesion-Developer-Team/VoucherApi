using Enum;

namespace DTOs.BodyDtos
{
    public class AssignCodesCollectionToPlayerBodyDto
    {
        public int? UnassignedCollectionId { get; set; }
        public int? PlayerId { get; set; }
        public DiscountType DiscountType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
