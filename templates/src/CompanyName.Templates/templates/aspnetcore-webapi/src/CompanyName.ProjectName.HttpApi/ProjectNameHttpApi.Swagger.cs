using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CompanyName.ProjectName
{
    internal static class SwaggerProjectNameHttpApi
    {

        internal static IServiceCollection AddProjectNameSwagger(this IServiceCollection services)
        {
            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "CompanyName ProjectName API", Version = "v1" });
                });

            return services;
        }
    }
}
