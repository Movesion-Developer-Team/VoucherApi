using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class AgencyConfiguration : IEntityTypeConfiguration<Agency>
    {
        public void Configure(EntityTypeBuilder<Agency> builder)
        {
            builder.Property(a => a.Name).HasMaxLength(100).IsRequired();
            builder.Property(a => a.ContactDate).IsRequired();

        

            builder.HasMany(a => a.Categories)
                .WithMany(c => c.Agencies)
                .UsingEntity<AgencyCategory>(j =>
                    {
                        j.HasOne(pt => pt.Agency)
                            .WithMany(a => a.AgencyCategories)
                            .HasForeignKey(pt => pt.AgencyId);
                        j.HasOne(pt => pt.Category)
                            .WithMany(c => c.AgencyCategories)
                            .HasForeignKey(pt => pt.CategoryId);
                        j.HasKey(pt => new
                        {
                            pt.AgencyId,
                            pt.CategoryId
                        });
                    });

            builder.HasMany(a => a.Players)
                .WithMany(p => p.Agencies)
                .UsingEntity<AgencyPlayer>(j =>
                {
                    j.HasOne(pt => pt.Agency)
                        .WithMany(a => a.AgencyPlayers)
                        .HasForeignKey(pt => pt.AgencyId);
                    j.HasOne(pt => pt.Player)
                        .WithMany(p => p.AgencyPlayers)
                        .HasForeignKey(pt => pt.PlayerId);
                    j.HasKey(x => new
                    {
                        x.AgencyId,
                        x.PlayerId
                    });
                });

        }
    }
}

