using Microsoft.EntityFrameworkCore;
using CompanyName.EntityFrameworkCore;

namespace CompanyName.Banks.EntityFrameworkCore
{

    public class BanksDbContext : CompanyNameDbContext<BanksDbContext>
    {

        // Create DbSets
        public DbSet<BankAccount> BankAccounts { get; set; }

        /// <summary>
        /// Default constructors
        /// </summary>
        public BanksDbContext(DbContextOptions<BanksDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureBanks();
        }
    }
}
