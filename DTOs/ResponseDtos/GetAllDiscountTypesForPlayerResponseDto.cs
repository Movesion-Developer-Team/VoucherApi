using Core.Domain;
using DTOs.BodyDtos;
using Enum;

namespace DTOs.ResponseDtos
{
    public class GetAllDiscountTypesForPlayerResponseDto : BaseResponse
    {
        public IQueryable<DiscountTypeBodyDto>? DiscountTypes { get; set; }

    }
}
