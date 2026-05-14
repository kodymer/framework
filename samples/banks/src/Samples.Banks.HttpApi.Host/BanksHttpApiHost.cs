using CompanyName.AspNetCore.Routing.Extensions;
using Serilog;

namespace Samples.Banks
{

    public static class BanksHttpApiHost
    {
        public static IHostBuilder ConfigureBanksHttpApiHost(this IHostBuilder host, IConfiguration configuration)
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


        public static IServiceCollection AddBanksHttpApiHost(
            this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
        {
            services
                .AddBanksHttpApi(configuration)
                .AddBanksEntityFrameworkCore(configuration)
                .AddBanksDapper(configuration);

            services
                .AddBanksOpenTelemetry(environment, configuration)
                .AddBanksHealthChecks(configuration)
                .AddBanksBus(configuration)
                .AddBanksCors(configuration);

            return services;
        }

        public static WebApplication UseBanksHttpApiHost(this WebApplication app)
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
              .MapBanksHealthChecks();

            app
              .MapEndpoints();

            return app;
        }
    }
}
