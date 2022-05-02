using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetAllCompaniesResponseDto : BaseResponse
    {
        public IQueryable<CompanyBodyDto>? Companies { get; set; }

    }
}
