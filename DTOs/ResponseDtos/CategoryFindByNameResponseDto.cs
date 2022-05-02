using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class CategoryFindByNameResponseDto : BaseResponse
    {
        public IQueryable<CategoryBodyDto>? Categories { get; set; }
    }
}
