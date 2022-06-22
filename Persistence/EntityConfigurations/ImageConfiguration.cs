using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<BaseImage>
    {
        public void Configure(EntityTypeBuilder<BaseImage> builder)
        {
            builder.HasOne(i => i.Category)
                .WithOne(c => c.Image)
                .HasForeignKey<BaseImage>(c => c.CategoryId);

            builder.HasOne(i => i.Player)
                .WithOne(p => p.Image)
                .HasForeignKey<BaseImage>(i => i.PlayerId);
        }
    }
}
