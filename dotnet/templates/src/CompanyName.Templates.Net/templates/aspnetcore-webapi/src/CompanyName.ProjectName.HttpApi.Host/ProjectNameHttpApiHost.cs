using CompanyName.AspNetCore.Routing.Extensions;
using Serilog;

namespace CompanyName.ProjectName
{

    public static class ProjectNameHttpApiHost
    {
        public static IHostBuilder ConfigureProjectNameHttpApiHost(this IHostBuilder host, IConfiguration configuration)
        {
            host
                .UseAutofac()
                .UseSerilog((context, services, logger) =>
                {
                    logger
                        .ReadFrom.Configuration(configuration)
                        .ReadFrom.Services(services);
                });

            return host;
        }


        public static IServiceCollection AddProjectNameHttpApiHost(
            this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
        {
            services
                .AddProjectNameHttpApi(configuration)
                .AddProjectNameEntityFrameworkCore(configuration)
                .AddProjectNameDapper(configuration);

            services
                .AddProjectNameOpenTelemetry(environment, configuration)
                .AddProjectNameHealthChecks(configuration)
                .AddProjectNameCors(configuration)
                .AddProjectNameBus(configuration);

            return services;
        }

        public static WebApplication UseProjectNameHttpApiHost(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app
                  .UseDeveloperExceptionPage();

                app
                  .UseSwagger()
                  .UseSwaggerUI();
            }
            else
            {
                // The default HSTS value is 30 days.
                // You may want to change this for production
                // scenarios, see https://aka.ms/aspnetcore-hsts.
                app
                  .UseHsts();
            }

            app
              .UseRouting();

            app
              .UseRequestLocalization(options =>
              {
                  var supportedCultures = new[] { "es", "en" };

                  options
                    .SetDefaultCulture(supportedCultures[0])
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);
              });

            app
              .UseCors("default");

            app
             .UseHttpsRedirection();

            app
              .MapProjectNameHealthChecks();

            app
              .MapEndpoints();

            return app;
        }
    }
}
