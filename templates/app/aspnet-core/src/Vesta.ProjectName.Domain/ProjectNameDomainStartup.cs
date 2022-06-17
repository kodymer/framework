using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vesta.ProjectName.Configuration;

namespace Vesta.ProjectName
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
