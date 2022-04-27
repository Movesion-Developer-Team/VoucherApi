using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class CompanyFindByNameResponseDto : BaseResponse
    {
        public IQueryable<CompanyBodyDto>? Unit { get; set; }
    }
}
