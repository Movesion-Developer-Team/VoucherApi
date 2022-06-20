using Enum;

namespace Core.Domain
{
    public class Discount : EntityBase
    {
        public string? Name { get; set; }
        public string? LinkTermsAndConditions { get; set; }
        public string? InfoCondizioni { get; set; }
        public string? InfoOttieni { get; set; }
        public string? InfoTermini { get; set; }
        public string? InfoAPaginaAcquisizione { get; set; }
        public UnitiesOfMeasurement? UnityOfMeasurement { get; set; }
        public float? DiscountValue { get; set; }
        public long? InitialPrice { get; set; }
        public long? FinalPrice { get; set; }
        public int? DiscountTypeId { get; set; }
        public int? PlayerId { get; set; }
        public int? PriceInPoints { get; set; }
        
        public ICollection<CompanyPortfolio>? CompanyPortfolios { get; set; }
        public ICollection<Company>? Companies { get; set; }
        public DiscountType? DiscountType { get; set; }
        public Player? Player { get; set; }
        public ValidityPeriod? ValidityPeriod { get; set; }
        public ICollection<DiscountCode>? DiscountCodes { get; set; }

    }
}
