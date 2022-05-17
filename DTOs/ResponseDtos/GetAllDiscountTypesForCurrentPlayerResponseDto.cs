using Core.Domain;
using DTOs.BodyDtos;
using Enum;

namespace DTOs.ResponseDtos
{
    public class GetAllDiscountTypesForCurrentPlayerResponseDto : BaseResponse
    {
        public IEnumerable<DiscountTypeBodyDto>? DiscountTypes { get; set; }

    }
}
