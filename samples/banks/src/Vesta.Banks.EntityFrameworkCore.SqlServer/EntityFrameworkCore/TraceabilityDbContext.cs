using Microsoft.EntityFrameworkCore;
using Vesta.Banks.Traceability;
using Vesta.EntityFrameworkCore;

namespace Vesta.Banks.EntityFrameworkCore
{
    public class TraceabilityDbContext : VestaDbContext<TraceabilityDbContext>
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
