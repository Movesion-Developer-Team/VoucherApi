using Core.Domain;

namespace DTOs.ResponseDtos
{
    public class GetAllPlayersForCurrentCompanyResponseDto : BaseResponse
    {
        public List<Player>? Players { get; set; }
    }
}
