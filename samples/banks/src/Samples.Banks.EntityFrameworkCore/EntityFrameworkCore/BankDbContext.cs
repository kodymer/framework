using CompanyName.Messaging.MassTransit.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Samples.Banks.Accounts;

namespace Samples.Banks.EntityFrameworkCore
{

    public class BankDbContext : CompanyNameOutboxDbContext<BankDbContext>
    {

        // Create DbSets
        public DbSet<BankAccount> BankAccounts { get; set; }

        /// <summary>
        /// Default constructors
        /// </summary>
        public BankDbContext(DbContextOptions<BankDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureBanks();
        }
    }
}
