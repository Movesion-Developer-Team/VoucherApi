using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.ValueGenerators;

namespace Persistence.EntityConfigurations
{
    public class SystemUpdateConfiguration : IEntityTypeConfiguration<SystemUpdate>
    {
        public void Configure(EntityTypeBuilder<SystemUpdate> builder)
        {
            builder.Property(su => su.UpdateDate)
                .ValueGeneratedOnAdd()
                .HasValueGenerator<SystemUpdateDateGenerator>();
        }
    }
}
