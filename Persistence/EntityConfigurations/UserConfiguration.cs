using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(w => w.Company)
                .WithMany(c => c.Users)
                .HasForeignKey(u=>u.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(u => u.JoinRequest)
                .WithOne(jr => jr.User);
            builder.HasIndex(c => c.IdentityUserId).IsUnique();
            builder.HasMany(u => u.Purchases)
                .WithOne(p => p.User);
            builder.HasMany(u => u.DiscountCodes)
                .WithOne(dc => dc.User);
        }
    }
}
