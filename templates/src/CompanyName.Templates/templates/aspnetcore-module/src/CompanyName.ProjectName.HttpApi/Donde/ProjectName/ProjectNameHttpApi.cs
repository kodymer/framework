using CompanyName.ProjectName;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace Donde.ProjectName
{
    public static class ProjectNameHttpApi
    {
        public static IServiceCollection AddProjectNameHttpApi(this IServiceCollection services, IConfiguration configuration)
        {

            services
                .AddProjectNameApplication(configuration);

            services
                .AddProjectNameValidators();

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
