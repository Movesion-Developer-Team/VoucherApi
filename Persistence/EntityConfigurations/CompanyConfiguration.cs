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

            builder.HasMany(c => c.Players)
                .WithMany(p => p.Companies)
                .UsingEntity<CompanyPlayer>(c =>
                {
                    c.HasOne(c => c.Company)
                        .WithMany(c => c.CompanyPlayers)
                        .HasForeignKey(c => c.CompanyId);
                    c.HasOne(c => c.Player)
                        .WithMany(p => p.CompanyPlayers)
                        .HasForeignKey(c => c.PlayerId);
                    c.HasKey(c => new
                    {
                        c.CompanyId,
                        c.PlayerId
                    });
                });
            
            builder.HasMany(c => c.Users)
                .WithOne(w => w.Company)
                .HasForeignKey(w => w.CompanyId)
                .OnDelete(DeleteBehavior.ClientCascade);
            builder.HasMany(c => c.InvitationCodes)
                .WithOne(ic => ic.Company)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasMany(c => c.Discounts)
                .WithMany(d => d.Companies)
                .UsingEntity<CompanyPortfolio>(cd =>
                {
                    cd.HasOne(cd => cd.Company)
                        .WithMany(c => c.CompanyPortfolios)
                        .HasForeignKey(cd => cd.CompanyId);

                    cd.HasOne(cd => cd.Discount)
                        .WithMany(d => d.CompanyPortfolios)
                        .HasForeignKey(cd => cd.DiscountId);
                    cd.HasAlternateKey(key=>new
                    {
                        key.CompanyId,
                        key.DiscountId
                    });
                });
        }
    }
}

