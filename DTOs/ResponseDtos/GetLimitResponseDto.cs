using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetLimitResponseDto : BaseResponse
    {
        public LimitBodyDto? Limit { get; set; }
    }
}
