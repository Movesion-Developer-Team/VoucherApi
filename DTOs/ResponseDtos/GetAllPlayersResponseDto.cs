using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetAllPlayersResponseDto : BaseResponse
    {
        public IQueryable<PlayerWithCategoriesAndDiscountTypesBodyDto>? Players { get; set; }
    }
}
