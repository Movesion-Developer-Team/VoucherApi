using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetAllPlayersResponseDto : BaseResponse
    {
        public IQueryable<PlayerBodyDto>? Unit { get; set; }
    }
}
