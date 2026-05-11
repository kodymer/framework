using CompanyName.ProjectName;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.ProjectName
{
    public static class ProjectNameDomain
    {

        public static IServiceCollection AddProjectNameDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddProjectNameDomainShared()
                .AddProjectNameDomainServices();

            services
                .AddCompanyNameLocalization();

            return services;
        }

    }
}
