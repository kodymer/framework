using CompanyName.AspNetCore.Routing.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Samples.Banks
{
    internal static class EndpointBanksHttpApi
    {
        internal static IServiceCollection AddBanksEndpoints(this IServiceCollection services)
        {

            services
                .AddEndpoints();

            return services;
        }
    }
}