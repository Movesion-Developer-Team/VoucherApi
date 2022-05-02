using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class CompanyFindByNameResponseDto : BaseResponse
    {
        public IQueryable<CompanyBodyDto>? Companies { get; set; }
    }
}
