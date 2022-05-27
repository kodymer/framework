using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Vesta.ApplicationInsights.AspNetCore.Extensions;
using Vesta.ApplicationInsights.AspNetCore.TelemetryInitializers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        private const string ApplicationInsightsSectionFromConfig = "ApplicationInsights";
        private const string RoleNameFromConfig = "ApplicationInsights:RoleName";
        private const string RoleNameEnvironmentVariable = "APPLICATIONINSIGHTS_ROLENAME";

        public static IServiceCollection AddVestaApplicationInsightsTelemetry(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry();
            services.Configure((VestaApplicationInsightsServiceOptions options) => AddTelemetryConfiguration(configuration, options));
            services.AddSingleton<ITelemetryInitializer, DomainNameRoleNameTelemetryInitializer>();

            return services;
        }

        private static void AddTelemetryConfiguration(
                IConfiguration config,
                VestaApplicationInsightsServiceOptions serviceOptions)
        {
            try
            {
                config.GetSection(ApplicationInsightsSectionFromConfig).Bind(serviceOptions);

                if (config.TryGetValue(primaryKey: RoleNameFromConfig, backupKey: RoleNameEnvironmentVariable, value: out string roleNameValue))
                {
                    serviceOptions.RoleName = roleNameValue;
                }
            }
            catch (Exception)
            {
                // TODO: log error
            }
        }

        private static bool TryGetValue(this IConfiguration config, string primaryKey, out string value, string backupKey = null)
        {
            value = config[primaryKey];

            if (backupKey != null && string.IsNullOrWhiteSpace(value))
            {
                value = config[backupKey];
            }

            return !string.IsNullOrWhiteSpace(value);
        }


    }
}
