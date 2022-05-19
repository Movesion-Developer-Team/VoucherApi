﻿using Core.Domain;
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
            
            builder.HasMany(c => c.Users)
                .WithOne(w => w.Company)
                .HasForeignKey(w => w.CompanyId);
            builder.HasMany(c => c.InvitationCodes)
                .WithOne(ic => ic.Company);

            builder.HasMany(c=>c.DiscountCodes)
                .WithMany(dc=>dc.Companies)
                .UsingEntity<CompanyDiscountCode>(ent =>
                {
                    ent.HasOne(cdc => cdc.Company)
                        .WithMany(c => c.CompanyDiscountCodes)
                        .HasForeignKey(cdc => cdc.CompanyId);
                    ent.HasOne(cdc => cdc.DiscountCode)
                        .WithMany(dc => dc.CompanyDiscountCodes)
                        .HasForeignKey(cdc => cdc.DiscountCodeId);
                    ent.HasKey(key => new
                    {
                        key.CompanyId,
                        key.DiscountCodeId
                    });
                });
        }
    }
}

