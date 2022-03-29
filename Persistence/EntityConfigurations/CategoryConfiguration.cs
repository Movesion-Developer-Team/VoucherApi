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
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);
            builder.HasMany(c => c.Vouchers)
                .WithOne(v => v.Category)
                .HasForeignKey(v => v.CategoryId);
            builder.HasMany(c => c.Agencies)
                .WithMany(a => a.Categories)
                .UsingEntity<AgencyCategory>(j =>
                {
                    j.HasOne(pt => pt.Category)
                        .WithMany(c => c.AgencyCategories)
                        .HasForeignKey(pt=>pt.CategoryId);
                    j.HasOne(pt => pt.Agency)
                        .WithMany(a => a.AgencyCategories)
                        .HasForeignKey(pt => pt.AgencyId);
                    j.HasKey(t => new
                    {
                        t.CategoryId,
                        t.AgencyId
                    });
                });

        }
    }
}
