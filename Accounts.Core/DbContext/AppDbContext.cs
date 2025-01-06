using Accounts.Core.Models;
using Accounts.Core.Models.Response;
using BaseClassLibrary;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Core.DbContext
{
    public class AppDbContext : GenericDbContext<AppDbContext>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Add DbSet properties for your entities
        public DbSet<Customer> CustomerMaster { get; set; }
        public DbSet<PurchaseMaster> PurchaseMaster { get; set; }
        public DbSet<PurchaseDetails> PurchaseDetails { get; set; }
        public DbSet<SalesMaster>  SalesMasters { get; set; }
        public DbSet<SalesDetails> SalesDetails { get; set; }
        public DbSet<AmountReceived> AmountReceived { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<UserMaster> UserMaster { get; set; }
        public DbSet<BrokerMaster> BrokerMaster { get; set; }
        public DbSet<ItemMaster> ItemMaster { get; set; }
        public DbSet<PurchaseReports> PurchaseReports { get; set; }
        public DbSet<PermissionMaster> PermissionMaster { get; set; }
        public DbSet<UserPermissionChild> UserPermissionChild { get; set; }
        public DbSet<StockReport> StockReport { get; set; }
        public DbSet<SaleReport> SaleReport { get; set; }
        public DbSet<CustomerReport> CustomerReport { get; set; }
        public DbSet<UserReport> UserReport { get; set; }
        public DbSet<POSMaster> POSMaster { get; set; }
        public DbSet<SeriesMaster> SeriesMaster { get; set; }
        public DbSet<POSChild> POSChild { get; set; }
        public DbSet<POSResponceModel> POSResponceModel { get; set; }


        // You can override OnModelCreating if you need additional configurations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Additional configurations for your entities
            modelBuilder.Entity<PurchaseReports>().HasNoKey();
            modelBuilder.Entity<StockReport>().HasNoKey();
            modelBuilder.Entity<SaleReport>().HasNoKey();
            modelBuilder.Entity<POSResponceModel>().HasNoKey();
            modelBuilder.Entity<CustomerReport>().HasNoKey();
            modelBuilder.Entity<UserReport>().HasNoKey();
        }
    }
}
