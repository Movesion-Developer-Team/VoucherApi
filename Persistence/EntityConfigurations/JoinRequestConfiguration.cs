using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class JoinRequestConfiguration : IEntityTypeConfiguration<JoinRequest>
    {
        public void Configure(EntityTypeBuilder<JoinRequest> builder)
        {
            builder.Property(jr => jr.Declined).HasDefaultValue(false);
            builder.HasOne(jr => jr.InvitationCode)
                .WithOne(ic => ic.JoinRequest)
                .HasForeignKey<JoinRequest>(jr => jr.InvitationCodeId);
            builder.HasOne(jr => jr.User)
                .WithOne(u => u.JoinRequest)
                .HasForeignKey<JoinRequest>(j => j.UserId);
        }
    }
}
