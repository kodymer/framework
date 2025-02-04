using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.ProjectName.Configuration
{
    public static class ApplicationInsightsConfiguration
    {
        public static void AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddApplicationInsightsKubernetesEnricher();
            services.AddCompanyNameApplicationInsightsTelemetry(configuration, options =>
            {
                options.EnableEventCounterCollectionModule = false;
                options.EnablePerformanceCounterCollectionModule = false;
                options.EnableActiveTelemetryConfigurationSetup = true;
                options.EnableHeartbeat = false;
            });

        }
    }
}