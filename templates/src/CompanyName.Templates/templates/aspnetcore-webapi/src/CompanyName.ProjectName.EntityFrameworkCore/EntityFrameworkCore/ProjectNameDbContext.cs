using CompanyName.Messaging.MassTransit.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.ProjectName.EntityFrameworkCore
{

    public class ProjectNameDbContext : CompanyNameOutboxDbContext<ProjectNameDbContext>
    {

        // Create DbSets here


        /// <summary>
        /// Default constructors
        /// </summary>
        public ProjectNameDbContext(DbContextOptions<ProjectNameDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Add delete global query filter

            modelBuilder.ConfigureProjectName();
        }
    }
}
