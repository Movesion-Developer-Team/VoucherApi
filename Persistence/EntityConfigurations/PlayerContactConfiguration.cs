using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class PlayerContactConfiguration : IEntityTypeConfiguration<PlayerContact>
    {
        public void Configure(EntityTypeBuilder<PlayerContact> builder)
        {
            builder.Property(pc => pc.PlayerId).IsRequired();
            builder.Property(pc => pc.Name).IsRequired();

            builder.HasOne(pc => pc.Player)
                .WithMany(p => p.PlayerContacts);
        }
    }
}
