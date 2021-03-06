

using System.Data.Common;
using System.Security.Principal;
using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityConfigurations;
using DiscountType = Core.Domain.DiscountType;

namespace Persistence
{
    public class VoucherContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerContact> PlayerContacts { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<InvitationCode> InvitationCodes { get; set; }
        public DbSet<JoinRequest> JoinRequests { get; set; }
        public DbSet<BaseImage> Images { get; set; }
        public DbSet<DiscountType> DiscountTypes { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<CompanyPortfolio> CompanyPortfolios { get; set; }
        public DbSet<SystemUpdate> SystemUpdates { get; set; }

        public VoucherContext(DbContextOptions<VoucherContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new CompanyConfiguration().Configure(modelBuilder.Entity<Company>());
            new CategoryConfiguration().Configure(modelBuilder.Entity<Category>());
            new DiscountConfiguration().Configure(modelBuilder.Entity<Discount>());
            new LocationConfiguration().Configure(modelBuilder.Entity<Location>());
            new PlayerConfiguration().Configure(modelBuilder.Entity<Player>());
            new PlayerContactConfiguration().Configure(modelBuilder.Entity<PlayerContact>());
            new ReportConfiguration().Configure(modelBuilder.Entity<Report>());
            new UserConfiguration().Configure(modelBuilder.Entity<User>());
            new InvitationCodeConfiguration().Configure(modelBuilder.Entity<InvitationCode>());
            new JoinRequestConfiguration().Configure(modelBuilder.Entity<JoinRequest>());
            new ImageConfiguration().Configure(modelBuilder.Entity<BaseImage>());
            new DiscountTypeConfiguration().Configure(modelBuilder.Entity<DiscountType>());
            new PurchaseConfiguration().Configure(modelBuilder.Entity<Purchase>());
            new DiscountCodeConfiguration().Configure(modelBuilder.Entity<DiscountCode>());
            new BatchConfiguration().Configure(modelBuilder.Entity<Batch>());
            new CompanyPortfolioConfiguration().Configure(modelBuilder.Entity<CompanyPortfolio>());
            new SystemUpdateConfiguration().Configure(modelBuilder.Entity<SystemUpdate>());
        }

    }

}
