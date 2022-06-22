namespace DTOs.ResponseDtos
{
    public class PaymentIntentResponseDto : BaseResponse
    {
        public string? ClientSecret { get; set; }
    }
}
