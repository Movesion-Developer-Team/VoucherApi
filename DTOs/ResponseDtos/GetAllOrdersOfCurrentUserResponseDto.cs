using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetAllOrdersOfCurrentUserResponseDto : BaseResponse
    {
        public IQueryable<UserDiscountCodeBodyDto>? Codes { get; set; }
    }
}
