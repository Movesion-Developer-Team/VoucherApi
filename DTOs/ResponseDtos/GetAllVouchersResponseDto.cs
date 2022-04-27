using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetAllVouchersResponseDto : BaseResponse
    {
        public IQueryable<VoucherBodyDto>? Unit { get; set; }
    }
}
