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
                .WithOne(d => d.DiscountCode)
                .HasForeignKey<Discount>(d=>d.DiscountCodeId);

            builder.HasOne(dc => dc.UnassignedDiscountCodeCollections)
                .WithMany(ua => ua.DiscountCodes);
        }
    }
}
