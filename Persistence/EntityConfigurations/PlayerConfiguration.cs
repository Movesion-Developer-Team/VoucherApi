using System.Drawing;
using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            
            builder.Property(p => p.Color).HasDefaultValue("Yellow");
            builder.Property(p => p.ShortName).IsRequired();

            builder.HasMany(p => p.Discounts)
                .WithOne(d => d.Player)
                .HasForeignKey(d=>d.PlayerId);

            builder.HasMany(p => p.Locations)
                .WithMany(l => l.Players)
                .UsingEntity<PlayerLocation>(j =>
                {
                    j.HasOne(pt => pt.Player)
                        .WithMany(p => p.PlayerLocations)
                        .HasForeignKey(pt => pt.PlayerId);
                    j.HasOne(pt=>pt.Location)
                        .WithMany(l=>l.PlayerLocations)
                        .HasForeignKey(pt=>pt.LocationId);
                    j.HasKey(pt => new
                    {
                        pt.PlayerId,
                        pt.LocationId
                    });
                });

            builder.HasMany(p => p.PlayerContacts)
                .WithOne(pc => pc.Player)
                .HasForeignKey(pc => pc.PlayerId);

            builder.HasMany(p => p.Categories)
                .WithMany(c => c.Players)
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


            builder.HasMany(p => p.Companies)
                .WithMany(a => a.Players)
                .UsingEntity<CompanyPlayer>(j =>
                {
                    j.HasOne(pt => pt.Company)
                        .WithMany(a => a.CompanyPlayers)
                        .HasForeignKey(pt=>pt.CompanyId);
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