using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vesta.ProjectName.Configuration
{
    public static class LoggingConfiguration
    {
        public static void AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationInsightsServiceOptions = new ApplicationInsightsServiceOptions
            {
                EnableEventCounterCollectionModule = false,
                EnablePerformanceCounterCollectionModule = false,
                EnableActiveTelemetryConfigurationSetup = true,
                EnableHeartbeat = false,
                InstrumentationKey = configuration.GetValue<string>("ApplicationInsights:InstrumentationKey")
            };

            services.AddApplicationInsightsTelemetry(applicationInsightsServiceOptions);
            services.AddApplicationInsightsKubernetesEnricher();
        }
    }
}