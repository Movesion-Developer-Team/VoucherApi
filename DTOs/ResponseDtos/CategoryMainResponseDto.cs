using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class CategoryMainResponseDto : BaseResponse
    {
        public CategoryBodyDto? Unit { get; set; }
    }
}
