using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class PlayerFindByNameResponseDto : BaseResponse
    {
        public IQueryable<PlayerOnlyBodyDto>? Players { get; set; }
    }
}
