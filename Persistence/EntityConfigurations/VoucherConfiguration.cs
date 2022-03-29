using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.Property(v => v.CategoryId).IsRequired();
            builder.Property(v => v.DiscountId).IsRequired();
            builder.Property(v => v.Name).IsRequired();

            builder.HasOne(v => v.Category)
                .WithMany(c => c.Vouchers);
            builder.HasOne(v => v.Discount)
                .WithMany(d => d.Vouchers);
        }
    }
}
