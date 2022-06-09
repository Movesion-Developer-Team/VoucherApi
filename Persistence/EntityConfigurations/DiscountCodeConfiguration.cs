using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class DiscountCodeConfiguration : IEntityTypeConfiguration<DiscountCode>
    {
        public void Configure(EntityTypeBuilder<DiscountCode> builder)
        {
            builder.Property(dc => dc.TemporaryReserved).HasDefaultValue(value: false);
            
            builder.HasMany(dc => dc.Purchases)
                .WithOne(p => p.DiscountCode);
            builder.HasOne(dc => dc.Batch)
                .WithMany(b => b.DiscountCodes)
                .HasForeignKey(dc => dc.BatchId);
            
            builder.HasOne(dc => dc.Player)
                .WithMany(p => p.DiscountCodes)
                .HasForeignKey(dc => dc.PlayerId);

            builder.HasOne(dc => dc.CompanyPortfolio)
                .WithMany(cp => cp.DiscountCodes)
                .HasForeignKey(dc=>dc.CompanyPortfolioId);

            builder.HasOne(dc => dc.User)
                .WithMany(u => u.DiscountCodes)
                .HasForeignKey(dc=>dc.UserId);

        }
    }
}
