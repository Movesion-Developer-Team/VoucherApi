using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetDiscountLimitResponseDto : BaseResponse
    {
        public LimitBodyDto? Limit { get; set; }
    }
}
