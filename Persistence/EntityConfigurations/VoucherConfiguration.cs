using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            
            builder.Property(v => v.Name).IsRequired();

            builder.HasOne(v => v.DiscountCode)
                .WithOne(dc => dc.Voucher)
                .HasForeignKey<Voucher>(v => v.DiscountCodeId);
        }
    }
}
