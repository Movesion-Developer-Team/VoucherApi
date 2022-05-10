namespace DTOs.BodyDtos
{
    public class JoinRequestBodyDto : BaseBody
    {
        public string? Message { get; set; }
        public int? InvitationCodeId { get; set; }
        public int? UserId { get; set; }
    }
}
