using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System.Linq;
using Vesta.EntityFrameworkCore.SqlServer;

namespace Vesta.ProjectName.EntityFrameworkCore
{

    public class ProjectNameDbContext : VestaDbContext<ProjectNameDbContext>
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
