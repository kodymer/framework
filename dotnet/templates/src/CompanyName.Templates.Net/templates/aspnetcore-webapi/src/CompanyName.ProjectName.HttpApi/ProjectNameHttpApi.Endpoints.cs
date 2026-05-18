using CompanyName.AspNetCore.Routing.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.ProjectName
{
    internal static class EndpointProjectNameHttpApi
    {
        internal static IServiceCollection AddProjectNameEndpoints(this IServiceCollection services)
        {

            services
                .AddEndpoints();

            return services;
        }
    }
}