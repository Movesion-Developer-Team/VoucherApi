using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetCurrentUserInfoResponseDto : BaseResponse
    {
        public BaseBody? CompanyId { get; set; }
    }
}
