using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class DiscountTypeConfiguration : IEntityTypeConfiguration<DiscountType>
    {
        public void Configure(EntityTypeBuilder<DiscountType> builder)
        {
            builder.HasMany(dt => dt.Discounts)
                .WithOne(d => d.DiscountType);

            builder.HasMany(d => d.Players)
                .WithMany(p => p.DiscountsTypes)
                .UsingEntity<PlayerDiscountType>(opt =>
                {
                    opt.HasOne(pd => pd.DiscountType)
                        .WithMany(dt => dt.PlayerDiscountTypes)
                        .HasForeignKey(pd => pd.DiscountTypeId);
                    opt.HasOne(pd => pd.Player)
                        .WithMany(dt => dt.PlayerDiscountTypes)
                        .HasForeignKey(pd => pd.PlayerId);
                    opt.HasKey(key => new
                    {
                        key.PlayerId,
                        key.DiscountTypeId
                    });
                });

            builder.HasMany(dt => dt.Batches)
                .WithOne(b => b.DiscountType);

        }
    }
}
