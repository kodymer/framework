using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
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
                .AddProjectNameAppSevices();

            services
                .AddVestaDddApplication();

            services
                .AddVestaCachingStackExchangeRedis(options =>
                {
                    options.Configuration = configuration.GetValue<string>(RedisConfigurationConfig);

                    // Delete, if you want to share the data between microservices 
                    options.InstanceName = "Vesta.ProjectName.";  //<--  Prefix key: Vesta.Banks.{Key}
                });

            services
                .AddVestaAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }


    }
}
