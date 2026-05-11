using Microsoft.Extensions.DependencyInjection;
using CompanyName.ProjectName.EntityFrameworkCore;

namespace CompanyName.ProjectName
{
    internal static class RepositoryProjectNameEntityFrameworkCore
    {

        internal static IServiceCollection AddProjectNameRepositories(this IServiceCollection services)
        {
            services.AddRepositories<ProjectNameDbContext>();

            return services;
        }
    }
}
