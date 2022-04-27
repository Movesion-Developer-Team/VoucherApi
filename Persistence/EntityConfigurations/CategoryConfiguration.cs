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


            builder.HasMany(ca => ca.Players)
                .WithOne(p => p.Category);

            builder.HasMany(c => c.Vouchers)
                .WithOne(v => v.Category)
                .HasForeignKey(v => v.CategoryId);

        }
    }
}
