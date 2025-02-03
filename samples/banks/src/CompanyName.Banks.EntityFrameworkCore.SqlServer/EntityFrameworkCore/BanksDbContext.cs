using Microsoft.EntityFrameworkCore;
using Vesta.EntityFrameworkCore;

namespace Vesta.Banks.EntityFrameworkCore
{

    public class BanksDbContext : VestaDbContext<BanksDbContext>
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
