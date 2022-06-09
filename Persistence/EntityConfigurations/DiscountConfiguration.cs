using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.OwnsOne(d => d.ValidityPeriod,
                vp =>
                {
                    vp.Property(vp => vp.StartDate).IsRequired();
                    vp.Property(vp => vp.EndDate).IsRequired();
                });
            

            
            builder.HasOne(d => d.Player)
                .WithMany(p => p.Discounts)
                .HasForeignKey(d=>d.PlayerId);

            builder.HasOne(d => d.DiscountType)
                .WithMany(dt => dt.Discounts)
                .HasForeignKey(d => d.DiscountTypeId);

            builder.HasMany(c => c.Companies)
                .WithMany(d => d.Discounts)
                .UsingEntity<CompanyPortfolio>(cd =>
                {
                    cd.HasOne(cd => cd.Company )
                        .WithMany(c => c.CompanyPortfolios)
                        .HasForeignKey(cd => cd.CompanyId);

                    cd.HasOne(cd => cd.Discount)
                        .WithMany(d => d.CompanyPortfolios)
                        .HasForeignKey(cd => cd.DiscountId);

                    cd.HasAlternateKey(key => new
                    {
                        key.CompanyId,
                        key.DiscountId
                    });
                });

        }
    }
}