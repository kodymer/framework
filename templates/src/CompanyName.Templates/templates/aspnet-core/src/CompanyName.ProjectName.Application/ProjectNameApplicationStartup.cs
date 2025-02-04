using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using CompanyName.ProjectName.Configuration;
using CompanyName.ProjectName.EntityFrameworkCore;

namespace CompanyName.ProjectName
{
    public static class ProjectNameApplicationStartup
    {
        public static IServiceCollection AddProjectNameApplication(this IServiceCollection services)
        {
            services
                .AddProjectNameDomain()
                .AddProjectNameEntityFrameworkCore()
                .AddProjectNameAppSevices();

            services
                .AddCompanyNameDddApplication();

            services
                .AddCompanyNameCachingStackExchangeRedis();

            services
                .AddCompanyNameAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }


    }
}
