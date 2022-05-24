namespace Core.Domain
{
    public class InvitationCode : EntityBase
    {
        public string? InviteCode { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? ExpireDate { get; set; }
        public int? CompanyId { get; set; }
        public int? JoinRequestId { get; set; }
        public Company? Company { get; set; }
        public JoinRequest? JoinRequest { get; set; }
    }
}
