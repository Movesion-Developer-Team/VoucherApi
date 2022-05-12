using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasOne(i => i.Category)
                .WithOne(c => c.Image)
                .HasForeignKey<Image>(c => c.CategoryId);

            builder.HasOne(i => i.Player)
                .WithOne(p => p.Image)
                .HasForeignKey<Image>(i => i.PlayerId);
        }
    }
}
