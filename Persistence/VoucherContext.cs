

using System.Security.Principal;
using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityConfigurations;

namespace Persistence
{
    public class VoucherContext : DbContext
    {
        
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerContact> PlayerContacts { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Report> Reports { get; set; }


        public VoucherContext(DbContextOptions<VoucherContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new AgencyConfiguration().Configure(modelBuilder.Entity<Agency>());
            new CategoryConfiguration().Configure(modelBuilder.Entity<Category>());
            new DiscountConfiguration().Configure(modelBuilder.Entity<Discount>());
            new LocationConfiguration().Configure(modelBuilder.Entity<Location>());
            new PlayerConfiguration().Configure(modelBuilder.Entity<Player>());
            new PlayerContactConfiguration().Configure(modelBuilder.Entity<PlayerContact>());
            new VoucherConfiguration().Configure(modelBuilder.Entity<Voucher>());
            new ReportConfiguration().Configure(modelBuilder.Entity<Report>());
        }

    }

}
