using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class PlayerMainResponseDto : BaseResponse
    {
        public PlayerBodyDto? Unit { get; set; }
    }
}
