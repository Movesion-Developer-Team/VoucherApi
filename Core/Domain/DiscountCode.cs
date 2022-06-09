namespace Core.Domain
{
    public class DiscountCode : EntityBase
    {
        public string? Code { get; set; }
        public int? BatchId { get; set; }
        public int? UsageLimit { get; set; }
        public bool? IsAssignedToUser { get; set; }
        public bool? IsAssignedToCompany { get; set; }
        public int? PlayerId { get; set; }
        public int? CompanyPortfolioId { get; set; }
        public int? UserId { get; set; }
        public bool? TemporaryReserved { get; set; }
        public CompanyPortfolio? CompanyPortfolio { get; set; }
        public Player? Player { get; set; }
        public Batch? Batch { get; set; }
        public User? User { get; set; }
        public ICollection<Purchase>? Purchases { get; set; }

    }
}
