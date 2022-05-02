using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class CompanyMainResponseDto : BaseResponse
    {
        public CompanyBodyDto? Company { get; set; }
    }
}
