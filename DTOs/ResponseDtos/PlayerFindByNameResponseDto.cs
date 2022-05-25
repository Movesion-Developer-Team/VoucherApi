using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class PlayerFindByNameResponseDto : BaseResponse
    {
        public IQueryable<PlayerWithCategoriesAndDiscountTypesBodyDto>? Players { get; set; }
    }
}
