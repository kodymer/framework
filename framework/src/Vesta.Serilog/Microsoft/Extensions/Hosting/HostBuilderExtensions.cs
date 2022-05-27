using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Microsoft.Extensions.Hosting
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseSerilog(this IHostBuilder hostBuilder, bool enableApplicationInsights = false)
        {
            return hostBuilder.UseSerilog((hostBuilderContext, serviceProvider, configureLogger) =>
            {
                if (enableApplicationInsights)
                {
                    var telemetryConfiguration = serviceProvider.GetRequiredService<TelemetryConfiguration>();

                    configureLogger
                        .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces);

                }
            });
        }
    }
}