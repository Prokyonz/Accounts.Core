using Accounts.Core.Models;
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
        public DbSet<AgentMaster> AgentMaster { get; set; }
        public DbSet<BrokerMaster> BrokerMaster { get; set; }


        // You can override OnModelCreating if you need additional configurations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Additional configurations for your entities
            //modelBuilder.Entity<CompanyMaster>().HasKey(c => c.Id);
        }
    }
}
