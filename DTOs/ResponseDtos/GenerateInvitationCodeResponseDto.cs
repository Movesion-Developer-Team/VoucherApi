namespace DTOs.ResponseDtos
{
    public class GenerateInvitationCodeResponseDto : BaseResponse
    {
        public int? InvitationCodeId { get; set; }
        public string? InvitationCode { get; set; }
    }
}
