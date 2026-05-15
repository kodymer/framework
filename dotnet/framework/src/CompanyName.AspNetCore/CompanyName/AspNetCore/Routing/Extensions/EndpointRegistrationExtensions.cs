using CompanyName.AspNetCore.Abstractions.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace CompanyName.AspNetCore.Routing.Extensions
{
    public static class EndpointRegistrationExtensions
    {
        public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();

            var endpointServices = assembly
                .DefinedTypes
                .Where(type => type.IsAssignableTo(typeof(IEndpoint)) && !type.IsAbstract && !type.IsInterface)
                .Select(type => new ServiceDescriptor(typeof(IEndpoint), type, ServiceLifetime.Transient))
                .ToArray();

            services.TryAddEnumerable(endpointServices);

            return services;
        }
    }
}
