using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetAllCategoriesResponseDto : BaseResponse
    {
        public IQueryable<CategoryBodyDto>? Categories { get; set; }
    }
}
