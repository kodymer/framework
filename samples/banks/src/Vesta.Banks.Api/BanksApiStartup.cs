using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace Vesta.Banks
{
    public static class BanksApiStartup
    {
        public static IServiceCollection AddBanksApi(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddBanksApplication(configuration);

            services
                .AddApiVersioning(options =>
                {
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                });

            services
                .AddVestaAspNetCoreMvc(jsonConfigure: options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            return services;
        }
    }
}
