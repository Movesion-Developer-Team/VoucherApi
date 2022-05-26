using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class OfferConfiguration : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.HasOne(o=>o.Company)
                .WithMany(c=>c.Offers)
                .HasForeignKey(o => o.CompanyId);

            builder.HasOne(o => o.DiscountCode)
                .WithMany(d => d.Offers)
                .HasForeignKey(o => o.DiscountCodeId);
        }
    }
}
