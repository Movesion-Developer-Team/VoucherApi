using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class CategoryMainResponseDto : BaseResponse
    {
        public CategoryBodyDto? Category { get; set; }
    }
}
