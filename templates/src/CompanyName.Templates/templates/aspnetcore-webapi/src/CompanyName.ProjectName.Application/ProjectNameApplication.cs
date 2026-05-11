using System.Reflection;
using CompanyName.Caching;
using Microsoft.Extensions.Caching.Hybrid;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.ProjectName
{
    public static class ProjectNameApplication
    {
        public static IServiceCollection AddProjectNameApplication(this IServiceCollection services, IConfiguration configuration)
        {

            services
                .AddProjectNameDomain(configuration)
                .AddProjectNameAppServices();

            services
                .AddCompanyNameAutoMapper()
                .AddCompanyNameHybridCaching()
                .UseRedis();

            return services;
        }

    }
}
