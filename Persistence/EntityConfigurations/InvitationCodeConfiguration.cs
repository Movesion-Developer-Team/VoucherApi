using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class InvitationCodeConfiguration : IEntityTypeConfiguration<InvitationCode>
    {
        public void Configure(EntityTypeBuilder<InvitationCode> builder)
        {
            builder.HasOne(ic => ic.Company)
                .WithMany(c => c.InvitationCodes)
                .HasForeignKey(ic => ic.CompanyId);
            builder.HasOne(ic => ic.JoinRequest)
                .WithOne(jr => jr.InvitationCode);
            
        }
    }
}
