using Microsoft.EntityFrameworkCore;
using CompanyName.Banks.Traceability;
using CompanyName.EntityFrameworkCore;

namespace CompanyName.Banks.EntityFrameworkCore
{
    public class TraceabilityDbContext : CompanyNameDbContext<TraceabilityDbContext>
    {

        // Create DbSets
        public DbSet<Error> Errors { get; set; }

        /// <summary>
        /// Default constructors
        /// </summary>
        public TraceabilityDbContext(DbContextOptions<TraceabilityDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureTraceability();
        }
    }
}
