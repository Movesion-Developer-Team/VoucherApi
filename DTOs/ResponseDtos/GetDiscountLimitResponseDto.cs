using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetDiscountLimitResponseDto : BaseResponse
    {
        public List<LimitBodyDto>? Limits { get; set; }
    }
}
