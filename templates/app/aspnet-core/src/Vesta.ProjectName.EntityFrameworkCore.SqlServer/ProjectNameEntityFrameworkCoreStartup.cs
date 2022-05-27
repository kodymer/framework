using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vesta.ProjectName.Configuration;

namespace Vesta.ProjectName.EntityFrameworkCore
{
    public static class ProjectNameEntityFrameworkCoreStartup
    {
        public static void AddProjectNameEntityFrameworkCore(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext(configuration)
                .AddRepositories();
        }
    }
}
