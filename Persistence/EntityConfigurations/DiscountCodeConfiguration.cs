using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.ValueGenerators;

namespace Persistence.EntityConfigurations
{
    public class DiscountCodeConfiguration : IEntityTypeConfiguration<DiscountCode>
    {
        public void Configure(EntityTypeBuilder<DiscountCode> builder)
        {
            builder.Property(dc => dc.TemporaryReserved).HasDefaultValue(value: false);
            builder.Property(dc => dc.IsAssignedToUser).HasDefaultValue(value: false);
            builder.Property(dc => dc.ReservationTime)
                .ValueGeneratedOnAddOrUpdate()
                .HasValueGenerator<ReservationTimeUnmanagedGenerator>();
            
            builder.HasMany(dc => dc.Purchases)
                .WithOne(p => p.DiscountCode);

            builder.HasOne(dc => dc.Batch)
                .WithMany(b => b.DiscountCodes)
                .HasForeignKey(dc => dc.BatchId);

            builder.HasOne(dc => dc.User)
                .WithMany(u => u.DiscountCodes)
                .HasForeignKey(dc=>dc.UserId);

            builder.HasOne(dc => dc.Discount)
                .WithMany(d => d.DiscountCodes)
                .HasForeignKey(dc=>dc.DiscountId);
        }
    }
}
