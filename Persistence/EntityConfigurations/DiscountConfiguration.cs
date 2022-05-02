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

            builder.Property(d => d.DiscountType)
                .HasConversion<int>()
                .IsRequired();
            builder.HasIndex(d => d.Code).IsUnique();
            builder.Property(d => d.NumberOfUsagePerCompany).IsRequired();
            builder.Property(d => d.NumberOfUsagePerUser).IsRequired();

            builder.HasOne(d => d.Player)
                .WithMany(p => p.Discounts);
            builder.HasMany(d => d.Vouchers)
                .WithOne(v => v.Discount)
                .HasForeignKey(v => v.DiscountId);

        }
    }
}