using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace Samples.Banks
{
    public static class BanksHttpApi
    {
        public static IServiceCollection AddBanksHttpApi(this IServiceCollection services, IConfiguration configuration)
        {

            services
                .AddBanksApplication(configuration);

            services
                .AddBanksSwagger()
                .AddBanksEndpoints()
                .AddBanksValidators();

            //services.AddApiVersioning(options =>
            //{
            //    options.DefaultApiVersion = new ApiVersion(1);
            //    options.ReportApiVersions = true;
            //    options.AssumeDefaultVersionWhenUnspecified = true;
            //    options.ApiVersionReader = ApiVersionReader.Combine(
            //        new UrlSegmentApiVersionReader(),
            //        new HeaderApiVersionReader("X-Api-Version"));
            //})
            //.AddMvc() // This is needed for controllers
            //.AddApiExplorer(options =>
            //{
            //    options.GroupNameFormat = "'v'V";
            //    options.SubstituteApiVersionInUrl = true;
            //});

            services
                .AddCompanyNameAspNetCoreMvc(jsonConfigure: options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            return services;
        }
    }
}