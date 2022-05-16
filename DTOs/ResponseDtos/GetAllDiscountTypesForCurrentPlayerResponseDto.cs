using Enum;

namespace DTOs.ResponseDtos
{
    public class GetAllDiscountTypesForCurrentPlayerResponseDto : BaseResponse
    {
        public IEnumerable<DiscountType>? DiscountTypes { get; set; }

    }
}
