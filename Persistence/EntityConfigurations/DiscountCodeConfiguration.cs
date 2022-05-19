using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class DiscountCodeConfiguration : IEntityTypeConfiguration<DiscountCode>
    {
        public void Configure(EntityTypeBuilder<DiscountCode> builder)
        {
            builder.HasIndex(d => d.Code).IsUnique();

            builder.HasOne(dc => dc.Discount)
                .WithMany(d => d.DiscountCodes)
                .HasForeignKey(dc=>dc.DiscountId);
            builder.HasMany(dc => dc.Companies)
                .WithMany(c => c.DiscountCodes)
                .UsingEntity<CompanyDiscountCode>(ent =>
                {
                    ent.HasOne(cdc => cdc.Company)
                        .WithMany(c => c.CompanyDiscountCodes)
                        .HasForeignKey(cdc => cdc.CompanyId);
                    ent.HasOne(cdc => cdc.DiscountCode)
                        .WithMany(dc => dc.CompanyDiscountCodes)
                        .HasForeignKey(cdc => cdc.DiscountCodeId);
                    ent.HasKey(key => new
                    {
                        key.CompanyId, 
                        key.DiscountCodeId
                    });
                });
            builder.HasMany(dc => dc.Purchases)
                .WithOne(p => p.DiscountCode);

        }
    }
}
