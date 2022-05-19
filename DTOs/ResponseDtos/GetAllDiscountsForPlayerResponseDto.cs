using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetAllDiscountsForPlayerResponseDto : BaseResponse
    {
        public IQueryable<DiscountBodyDto>? Discounts { get; set; }
    }
}
