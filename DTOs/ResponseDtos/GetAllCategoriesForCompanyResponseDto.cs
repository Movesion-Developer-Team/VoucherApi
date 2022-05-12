using Core.Domain;
using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetAllCategoriesForCompanyResponseDto : BaseResponse
    {
        public IQueryable<CategoryBodyDto>? Categories { get; set; }
    }
}
