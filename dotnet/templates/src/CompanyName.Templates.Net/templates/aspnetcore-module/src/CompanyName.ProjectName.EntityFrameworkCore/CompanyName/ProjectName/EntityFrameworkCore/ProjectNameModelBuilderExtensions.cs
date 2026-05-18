using Microsoft.EntityFrameworkCore;

namespace CompanyName.ProjectName.EntityFrameworkCore
{
    public static class ProjectNameModelBuilderExtensions
    {
        public static void ConfigureProjectName(this ModelBuilder modelBuilder)
        {
            /*
             *   Samples...
             *
             *   modelBuilder.Entity<MyAggregateRoot>(b =>
             *   {
             *       b.ToTable("MyTableName");
             *       b.HasKey(p => p.Id);
             *       
             *       b.ConfigureFullAuditedAggregateRoot();
             *   });
             *
             *
             */
        }
    }
}
