namespace Core.Domain
{
    public class User : EntityBase
    {
        public string? IdentityUserId { get; set; }
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
