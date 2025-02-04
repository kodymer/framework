using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CompanyName.ProjectName.EntityFrameworkCore;

namespace CompanyName.ProjectName.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddProjectNameDbContext(this IServiceCollection services)
        {
            services.AddCompanyNameDbContext<ProjectNameDbContext>();

            return services;
        }
    }
}
