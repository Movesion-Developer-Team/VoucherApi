using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetAllCategoriesForPlayerResponseDto : BaseResponse
    {
        public IQueryable<CategoryBodyDto>? Categories { get; set; }
    }
}
