using Core.Domain;
using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GHetAllCategoriesForCompanyResponseDto : BaseResponse
    {
        public IQueryable<CategoryBodyDto>? Categories { get; set; }
    }
}
