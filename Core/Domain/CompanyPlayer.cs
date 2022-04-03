namespace Core.Domain
{
    public class CompanyPlayer : EntityBase
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }

    }
}
