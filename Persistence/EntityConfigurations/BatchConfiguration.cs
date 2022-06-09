using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class BatchConfiguration : IEntityTypeConfiguration<Batch>
    {
        public void Configure(EntityTypeBuilder<Batch> builder)
        {
            builder.HasMany(b => b.DiscountCodes)
                .WithOne(dc => dc.Batch);

            builder.HasOne(b => b.DiscountType)
                .WithMany(dt => dt.Batches)
                .HasForeignKey(b => b.DiscountTypeId);
        }
    }
}
