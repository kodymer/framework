using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vesta.ProjectName
{
    public static class ProjectNameApiStartup
    {
        public static void AddProjectNameApi(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddProjectNameApplication(configuration);

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddApplicationInsightsTelemetry(configuration);

        }
    }
}
