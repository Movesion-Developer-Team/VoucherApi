using Core.Domain;
using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetAllPlayersForCurrentCompanyResponseDto : BaseResponse
    {
        public IQueryable<PlayerBodyDto>? Players { get; set; }
    }
}
