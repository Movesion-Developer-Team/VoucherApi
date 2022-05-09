using Core.Domain;
using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetAllUsersForCompanyResponseDto : BaseResponse
    {
        public IEnumerable<User>? Users { get; set; }
    }
}
