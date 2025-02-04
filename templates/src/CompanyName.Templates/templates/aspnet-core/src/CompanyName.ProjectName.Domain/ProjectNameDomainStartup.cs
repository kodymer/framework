using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CompanyName.ProjectName.Configuration;

namespace CompanyName.ProjectName
{
    public static class ProjectNameDomainStartup
    {
        public static IServiceCollection AddProjectNameDomain(this IServiceCollection services)
        {
            services
                .AddProjectNameDomainSevices();

            return services;
        }
    }
}
