using CompanyName.AspNetCore.Abstractions.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CompanyName.AspNetCore.Routing.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static WebApplication MapEndpoints(this WebApplication app)
        {
            var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

            var grouped = endpoints.GroupBy(e =>
            {
                var attr = e.GetType().GetCustomAttribute<EndpointGroupAttribute>();
                return attr?.Name ?? string.Empty;
            });

            foreach (var group in grouped)
            {
                IEndpointRouteBuilder builder;

                if (string.IsNullOrEmpty(group.Key))
                {
                    builder = app;
                }
                else
                {
                    builder = app.MapGroup($"/api/{group.Key}")
                                 .WithTags(group.Key); // opcional
                }

                foreach (var endpoint in group)
                {
                    endpoint.Map(builder);
                }
            }

            return app;
        }

    }
}
