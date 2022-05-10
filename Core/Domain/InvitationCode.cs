namespace Core.Domain
{
    public class InvitationCode : EntityBase
    {
        public string? InviteCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int? CompanyId { get; set; }
        public int? JoinRequestId { get; set; }
        public Company? Company { get; set; }
        public JoinRequest? JoinRequest { get; set; }
    }
}
