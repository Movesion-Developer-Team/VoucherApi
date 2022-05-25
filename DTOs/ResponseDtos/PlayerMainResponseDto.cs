using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class PlayerMainResponseDto : BaseResponse
    {
        public PlayerWithCategoriesAndDiscountTypesBodyDto? Player { get; set; }
    }
}
