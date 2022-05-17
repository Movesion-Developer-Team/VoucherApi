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

            builder.HasOne(dc => dc.UnassignedDiscountCodesCollection)
                .WithMany(ua => ua.DiscountCodes)
                .HasForeignKey(dc=>dc.UnassignedCollectionId);
            builder.HasOne(dc => dc.Voucher)
                .WithOne(v => v.DiscountCode);
        }
    }
}
