using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vesta.ProjectName.Configuration;

namespace Vesta.ProjectName.EntityFrameworkCore
{
    public static class ProjectNameEntityFrameworkCoreStartup
    {
        public static IServiceCollection AddProjectNameEntityFrameworkCore(this IServiceCollection services)
        {
            services
                .AddProjectNameDomain()
                .AddProjectNameDbContext()
                .AddProjectNameRepositories();

            return services;
        }
    }
}
