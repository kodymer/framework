using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System.Linq;
using Vesta.EntityFrameworkCore.SqlServer;
using Vesta.ProjectName.Bank;

namespace Vesta.ProjectName.EntityFrameworkCore
{

    public class ProjectNameDbContext : VestaDbContext<ProjectNameDbContext>
    {

        // Create DbSets
        public DbSet<BankAccount> BankAccounts { get; set; }

        /// <summary>
        /// Default constructors
        /// </summary>
        public ProjectNameDbContext(DbContextOptions<ProjectNameDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureProjectName();
        }
    }
}
