using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vesta.ProjectName.Configuration;
using Vesta.ProjectName.EntityFrameworkCore;

namespace Vesta.ProjectName
{
    public static class ProjectNameApplicationStartup
    {
        public static IServiceCollection AddProjectNameApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddProjectNameDomain(configuration)
                .AddProjectNameEntityFrameworkCore(configuration)
                .AddProjectNameAppSevices()
                .AddProjectNameAutoMapper();

            services
                .AddVestaDddApplication();

            return services;
        }


    }
}
