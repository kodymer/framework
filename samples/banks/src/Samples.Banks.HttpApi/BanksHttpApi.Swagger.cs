using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Samples.Banks
{
    internal static class SwaggerBanksHttpApi
    {

        internal static IServiceCollection AddBanksSwagger(this IServiceCollection services)
        {
            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());

                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Samples Banks API", Version = "v1" });
            });

            return services;

        }
    }
}
