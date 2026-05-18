using CompanyName.Data;
using HealthChecks.ApplicationStatus.DependencyInjection;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace CompanyName.ProjectName
{
    public static class HealthCheckProjectNameHttpApiHost
    {
        internal static IServiceCollection AddProjectNameHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var defaultTimeout = TimeSpan.FromSeconds(30);

            var sqlConnectionString = configuration.GetConnectionString("Default");

            var redisConnectionString = configuration["Redis:Configuration"];

            services
                .AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy("Process is alive"), tags: [ "live"])
                .AddApplicationStatus(name: "appstatus", tags: ["api", "ready"], timeout: defaultTimeout)
                .AddSqlServer(sqlConnectionString, name: "sqlserver", tags: ["db", "sql", "sqlserver", "ready"], timeout: defaultTimeout)
                .AddRedis(redisConnectionString, name: "redis", tags: ["db", "cache", "redis", "ready"], timeout: defaultTimeout);

            // NOTE:
            // If you enable MassTransit (services.AddSchedulingBus(configuration)), also enable the RabbitMQ health check.
            // This ensures /health/ready correctly reports the bus status for Kubernetes.
            //
            // .AddRabbitMQ(async (serviceProvider) => {
            // 
            //     var options = serviceProvider.GetRequiredService<IOptions<RabbitMqTransportOptions>>().Value;
            // 
            //     var factory = new ConnectionFactory()
            //     {
            //         UserName = options.User,
            //         Password = options.Pass,
            //         VirtualHost = options.VHost,
            //         HostName = options.Host,
            //         Port = options.Port
            //     };
            // 
            //     return await factory.CreateConnectionAsync();
            // 
            // }, name: "rabbitmq", tags: ["bus", "cache", "rabbitmq", "transport", "ready"], timeout: defaultTimeout);

            return services;
        }

        public static WebApplication MapProjectNameHealthChecks(this WebApplication app)
        {
            app.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live"),
                AllowCachingResponses = false
            });

            app.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("ready"),
                AllowCachingResponses = false
            });

            return app;
        }
    }
}
