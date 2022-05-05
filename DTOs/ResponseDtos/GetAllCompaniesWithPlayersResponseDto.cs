using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetAllCompaniesWithPlayersResponseDto : BaseResponse
    {
        public List<CompanyWithPlayersBodyDto>? Companies { get; set; }
    }
}
