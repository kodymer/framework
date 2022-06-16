using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vesta.ProjectName.EntityFrameworkCore;

namespace Vesta.ProjectName.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddProjectNameDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddVestaDbContext<ProjectNameDbContext>();

            return services;
        }
    }
}
