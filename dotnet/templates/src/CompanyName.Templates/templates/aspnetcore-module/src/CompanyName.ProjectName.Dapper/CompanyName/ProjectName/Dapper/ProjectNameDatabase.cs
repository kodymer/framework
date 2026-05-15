using CompanyName.Dapper;

namespace CompanyName.ProjectName.Dapper
{
    public class ProjectNameDatabase : CompanyNameDatabase<ProjectNameDatabase>
    {
        public ProjectNameDatabase()
        {

        }

        protected override void OnModelCreating(IModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureProjectName();
        }
    }
}
