using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.Property(l => l.City).IsRequired();
            builder.Property(l => l.PostalCode).IsRequired();
            builder.Property(l => l.Region).IsRequired();

            builder.HasMany(l => l.Players)
                .WithMany(p => p.Locations)
                .UsingEntity<PlayerLocation>(j =>
                {
                    j.HasOne(pt => pt.Player)
                        .WithMany(p => p.PlayerLocations)
                        .HasForeignKey(pt => pt.PlayerId);
                    j.HasOne(pt => pt.Location)
                        .WithMany(l => l.PlayerLocations)
                        .HasForeignKey(pt => pt.LocationId);
                    j.HasKey(k => new
                    {
                        k.LocationId,
                        k.PlayerId

                    });
                }); ;
        }
    }
}
