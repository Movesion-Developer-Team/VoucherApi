using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetAllPlayersResponseDto : BaseResponse
    {
        public IQueryable<PlayerBodyDto>? Players { get; set; }
    }
}
