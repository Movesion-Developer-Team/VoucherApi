namespace Core.Domain
{
    public class JoinRequest : EntityBase
    {
        public string? Message { get; set; }
        public int? InvitationCodeId { get; set; }
        public int? UserId { get; set; }
        public bool Declined { get; set; }
        public User? User { get; set; }
        public InvitationCode? InvitationCode { get; set; }

    }
}
