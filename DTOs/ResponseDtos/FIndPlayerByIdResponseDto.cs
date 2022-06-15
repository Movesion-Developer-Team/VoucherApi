using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class FindPlayerByIdResponseDto : BaseResponse
    {
        public List<PlayerWithCategoriesAndDiscountTypesBodyDto>? Players { get; set; }
    }
}
