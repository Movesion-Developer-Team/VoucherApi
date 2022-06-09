﻿using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class CompanyPortfolioConfiguration : IEntityTypeConfiguration<CompanyPortfolio>
    {
        public void Configure(EntityTypeBuilder<CompanyPortfolio> builder)
        {
            builder.HasMany(cp => cp.DiscountCodes)
                .WithOne(d => d.CompanyPortfolio);
        }
    }
}
