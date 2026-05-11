using CompanyName.ProjectName.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.ProjectName
{
    public static class ProjectNameEntityFrameworkCore
    {
        public static IServiceCollection AddProjectNameEntityFrameworkCore(this IServiceCollection services, IConfiguration configuration)
        {

            services
                .AddCompanyNameDbContext<ProjectNameDbContext>(sqlServerOptionAction: options =>
                {
                    options.TranslateParameterizedCollectionsToConstants();
                });

            services
                .AddProjectNameApplication(configuration)
                .AddProjectNameRepositories();

            return services;
        }
    }
}
