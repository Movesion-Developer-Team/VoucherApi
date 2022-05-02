using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder.Property(c => c.Name).HasMaxLength(100).IsRequired();


            builder.HasMany(c => c.Players)
                .WithMany(p => p.Categories)
                .UsingEntity<PlayerCategories>(j =>
                {
                    j.HasOne(pc => pc.Player)
                        .WithMany(p => p.PlayerCategories)
                        .HasForeignKey(pc => pc.PlayerId);
                    j.HasOne(pc => pc.Category)
                        .WithMany(c => c.PlayerCategories)
                        .HasForeignKey(pc => pc.CategoryId);

                    j.HasKey(pc => new
                    {
                        pc.PlayerId,
                        pc.CategoryId
                    });
                });

            builder.HasMany(c => c.Vouchers)
                .WithOne(v => v.Category)
                .HasForeignKey(v => v.CategoryId);

        }
    }
}
