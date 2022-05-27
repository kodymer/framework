using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vesta.ProjectName.Configuration;
using Vesta.ProjectName.EntityFrameworkCore;

namespace Vesta.ProjectName
{
    public static class ProjectNameApplicationStartup
    {
        public static void AddProjectNameApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddProjectNameDomain(configuration);
            services.AddProjectNameEntityFrameworkCore(configuration);

            services.AddAppSevices();
        }
    }
}
