using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.Property(a => a.Name).HasMaxLength(100).IsRequired();
            builder.Property(a => a.ContactDate).IsRequired();

        

            builder.HasMany(a => a.Categories)
                .WithMany(c => c.Companies)
                .UsingEntity<CompanyCategory>(j =>
                    {
                        j.HasOne(pt => pt.Company)
                            .WithMany(a => a.CompanyCategories)
                            .HasForeignKey(pt => pt.CompanyId);
                        j.HasOne(pt => pt.Category)
                            .WithMany(c => c.CompanyCategories)
                            .HasForeignKey(pt => pt.CategoryId);
                        j.HasKey(pt => new
                        {
                            AgencyId = pt.CompanyId,
                            pt.CategoryId
                        });
                    });

            builder.HasMany(a => a.Players)
                .WithMany(p => p.Companies)
                .UsingEntity<CompanyPlayer>(j =>
                {
                    j.HasOne(pt => pt.Company)
                        .WithMany(a => a.CompanyPlayers)
                        .HasForeignKey(pt => pt.CompanyId);
                    j.HasOne(pt => pt.Player)
                        .WithMany(p => p.CompanyPlayers)
                        .HasForeignKey(pt => pt.PlayerId);
                    j.HasKey(x => new
                    {
                        AgencyId = x.CompanyId,
                        x.PlayerId
                    });
                });

        }
    }
}

