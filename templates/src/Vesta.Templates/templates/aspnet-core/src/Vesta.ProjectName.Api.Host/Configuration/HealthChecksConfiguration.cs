using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Vesta.ProjectName.Configuration
{
    public static class HealthChecksConfiguration
    {
        private const string SelfName = "self";
        private const string ServicesTag = "services";

        public static IServiceCollection AddHealthChecks(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var defaultTimeout = TimeSpan.FromSeconds(30);

            services.AddHealthChecks()
                .AddCheck(
                    SelfName,
                    timeout: defaultTimeout,
                    check: () => HealthCheckResult.Healthy());
                //.AddSqlServer(configuration["SqlConnectionString"]);

            return services;
        }

        public static IEndpointRouteBuilder MapHealthChecks(this IEndpointRouteBuilder endpoint)
        {
            endpoint.MapHealthChecks("/self", new HealthCheckOptions
            {
                Predicate = registration => registration.Name == SelfName,
                AllowCachingResponses = false
            });

            endpoint.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = registration => registration.Tags.Contains(ServicesTag),
                AllowCachingResponses = false
            });

            return endpoint;
        }
    }
}