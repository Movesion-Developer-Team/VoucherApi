using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class PlayerFindByNameResponseDto : BaseResponse
    {
        public IQueryable<PlayerBodyDto>? Players { get; set; }
    }
}
