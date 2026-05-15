using Microsoft.EntityFrameworkCore;
using Samples.Banks.Traceability;
using CompanyName.EntityFrameworkCore;

namespace Samples.Banks.EntityFrameworkCore
{
    public class TraceabilityDbContext : CompanyNameDbContext<TraceabilityDbContext>
    {

        // Create DbSets
        public DbSet<ErrorRecord> Errors { get; set; }

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
