using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class DiscountCodeConfiguration : IEntityTypeConfiguration<DiscountCode>
    {
        public void Configure(EntityTypeBuilder<DiscountCode> builder)
        {

            builder.HasOne(dc => dc.Discount)
                .WithMany(d => d.DiscountCodes)
                .HasForeignKey(dc=>dc.DiscountId);
            builder.HasMany(dc => dc.Purchases)
                .WithOne(p => p.DiscountCode);
            builder.HasOne(dc => dc.Batch)
                .WithMany(b => b.DiscountCodes)
                .HasForeignKey(dc => dc.BatchId);
            builder.HasMany(dc => dc.Offers)
                .WithOne(o => o.DiscountCode);

        }
    }
}
