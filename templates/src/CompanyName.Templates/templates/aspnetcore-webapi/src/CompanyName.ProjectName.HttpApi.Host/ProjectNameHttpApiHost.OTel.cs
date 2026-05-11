using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;


namespace CompanyName.ProjectName
{
    public static class OTelProjectNameHttpApiHost
    {
        internal static IServiceCollection AddProjectNameOpenTelemetry(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
        {
            // TO-DO: Add instrumentation class for OpenTelemetry in Framework solution

            services
                .AddOpenTelemetry()
                .ConfigureResource(resource =>
                {
                    resource
                        .AddService(environment.ApplicationName);

                })
                .WithTracing(tracing =>
                {
                    tracing
                         .AddSource(environment.ApplicationName)
                         .AddAspNetCoreInstrumentation(options =>
                         {
                             options.RecordException = true;
                         })
                         .AddHttpClientInstrumentation(options =>
                         {
                             options.RecordException = true;
                         })
                         .AddEntityFrameworkCoreInstrumentation()
                         .AddRedisInstrumentation();

                    tracing
                         .AddOtlpExporter(); //Configure enviroment variable OTEL_EXPORTER_OTLP_ENDPOINT (Otel Collector URL)

                    if (environment.IsDevelopment())
                    {
                        tracing
                         .AddConsoleExporter();
                    }

                })

               // Add Metrics for ASP.NET Core and our custom metrics and export
               // to Prometheus. for more information see:
               // https://learn.microsoft.com/es-es/dotnet/core/diagnostics/built-in-metrics?view=aspnetcore-9.0
               .WithMetrics(metrics =>
               {
                   metrics
                       .AddMeter(environment.ApplicationName)
                       .AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation()
                       .AddRuntimeInstrumentation()
                       .AddMeter("Microsoft.AspNetCore.Hosting")
                       .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                       .AddMeter("System.Net.Http")
                       .AddMeter("System.Net.NameResolution");

                   metrics
                       .AddOtlpExporter(); //Configure enviroment variable OTEL_EXPORTER_OTLP_ENDPOINT (Otel Collector URL)

                   if (environment.IsDevelopment())
                   {
                       metrics
                        .AddConsoleExporter();
                   }
               });


            return services;
        }
    }
}
