﻿namespace Core.Domain
{
    public class Discount : EntityBase
    {
        public string? Name { get; set; }
        public string? LinkTermsAndConditions { get; set; }
        public string? UnityOfMeasurement { get; set; }
        public float? DiscountValue { get; set; }
        public int? InitialPrice { get; set; }
        public long? FinalPrice { get; set; }
        public int? DiscountTypeId { get; set; }
        public int? PlayerId { get; set; }
        
        public ICollection<CompanyPortfolio>? CompanyPortfolios { get; set; }
        public ICollection<Company>? Companies { get; set; }
        public DiscountType? DiscountType { get; set; }
        public Player? Player { get; set; }
        public ValidityPeriod? ValidityPeriod { get; set; }
        public ICollection<DiscountCode>? DiscountCodes { get; set; }

    }
}
