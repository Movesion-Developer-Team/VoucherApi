using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class VoucherMainResponseDto : BaseResponse
    {
        public VoucherBodyDto? Voucher { get; set; }
    }
}
