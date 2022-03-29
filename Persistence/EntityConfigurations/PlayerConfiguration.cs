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

            builder.Property(p => p.CategoryId).IsRequired();
            builder.Property(p => p.Color).HasDefaultValue(KnownColor.Yellow);
            builder.Property(p => p.FullName).IsRequired();
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

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Players);
            builder.HasMany(p => p.Agencies)
                .WithMany(a => a.Players)
                .UsingEntity<AgencyPlayer>(j =>
                {
                    j.HasOne(pt => pt.Agency)
                        .WithMany(a => a.AgencyPlayers)
                        .HasForeignKey(pt=>pt.AgencyId);
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