using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetAllDiscountTypesResponseDto : BaseResponse
    {
        public IQueryable<DiscountTypeBodyDto> DiscountTypes { get; set; }
    }
}
