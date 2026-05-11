using CompanyName.ProjectName.Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.ProjectName
{
    public static class ProjectNameDapper
    {
        public static IServiceCollection AddProjectNameDapper(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddCompanyNameDatabase<ProjectNameDatabase>();

            services
                .AddProjectNameRepositories()
                .AddProjectNameApplication(configuration);

            return services;
        }
    }
}
