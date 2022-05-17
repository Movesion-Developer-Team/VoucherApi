using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class UnassignedDiscountCodeCollectionsConfiguration : IEntityTypeConfiguration<UnassignedDiscountCodeCollection>
    {
        public void Configure(EntityTypeBuilder<UnassignedDiscountCodeCollection> builder)
        {
            builder.HasMany(ud => ud.DiscountCodes)
                .WithOne(dc => dc.UnassignedDiscountCodesCollection)
                .HasForeignKey(dc => dc.UnassignedCollectionId);
        }
    }
}
