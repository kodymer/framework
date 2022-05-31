using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vesta.Banks.Configuration
{
    public static class ApplicationInsightsConfiguration
    {
        public static void AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddApplicationInsightsKubernetesEnricher();
            services.AddVestaApplicationInsightsTelemetry(configuration, options =>
            {
                options.EnableEventCounterCollectionModule = false;
                options.EnablePerformanceCounterCollectionModule = false;
                options.EnableActiveTelemetryConfigurationSetup = true;
                options.EnableHeartbeat = false;
            });

        }
    }
}