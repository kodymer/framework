using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ApplicationInsights.Extensibility;
using System;

namespace Vesta.ProjectName
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File("Logs/logs.txt"))
#if DEBUG
            .WriteTo.Async(c => c.Console())
#endif
            .CreateBootstrapLogger();

            try
            {
                Log.Information("Starting Vesta.ProjectName.Api.Host.");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseAutofac()
                .UseSerilog((hostBuilderContext, serviceProvider, configureLogger) =>
                {

                     configureLogger
                         .ReadFrom.Configuration(hostBuilderContext.Configuration)
                         .ReadFrom.Services(serviceProvider)
                         .Enrich.FromLogContext()
                         .WriteTo.Async(c => c.File("Logs/logs.txt"))
#if DEBUG
                         .WriteTo.Async(c => c.Console())
#endif
                         .WriteTo.Async(c => c.ApplicationInsights(serviceProvider.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces));


                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<ProjectNameApiHostStartup>();
                });
    }
}
