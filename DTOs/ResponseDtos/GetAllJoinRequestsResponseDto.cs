using Core.Domain;
using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetAllJoinRequestsResponseDto : BaseResponse
    {
        public List<JoinRequestBodyDto>? JoinRequests { get; set; }
    }
}
