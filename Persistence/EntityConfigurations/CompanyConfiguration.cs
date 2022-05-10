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
            builder.HasMany(c => c.Users)
                .WithOne(w => w.Company)
                .HasForeignKey(w => w.CompanyId);
            builder.HasMany(c => c.InvitationCodes)
                .WithOne(ic => ic.Company);
        }
    }
}

