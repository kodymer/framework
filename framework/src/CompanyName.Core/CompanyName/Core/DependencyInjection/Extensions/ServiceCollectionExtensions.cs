using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CompanyName.Core.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddOrReplace<TServiceType, TImplemententionType>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            where TServiceType : class
            where TImplemententionType : class, TServiceType
        {
            if (!TryReplace<TServiceType, TImplemententionType>(services, serviceLifetime))
            {
                services.Add(new ServiceDescriptor(typeof(TServiceType), typeof(TImplemententionType), serviceLifetime));
            }
        }

        public static void AddOrReplace(this IServiceCollection services, Type serviceType, Type implemententionType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            if (!TryReplace(services, serviceType, implemententionType, serviceLifetime))
            {
                services.Add(new ServiceDescriptor(serviceType, implemententionType, serviceLifetime));
            }
        }

        public static IServiceCollection Replace<TServiceType, TImplemententionType>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            where TServiceType : class
            where TImplemententionType : class, TServiceType
        {
            if (!TryReplace<TServiceType, TImplemententionType>(services, serviceLifetime))
            {
                services.Add(new ServiceDescriptor(typeof(TServiceType), typeof(TImplemententionType), serviceLifetime));
            }

            return services;
        }

        public static bool TryReplace<TServiceType, TImplemententionType>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            where TServiceType : class
            where TImplemententionType : class, TServiceType
        {
            return TryReplace(services, typeof(TServiceType), typeof(TImplemententionType), serviceLifetime);
        }

        public static bool TryReplace(this IServiceCollection services, Type serviceType, Type implemententionType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            bool wasReplaced = false;

            if (Any(services, serviceType))
            {
                services.Replace(
                    new ServiceDescriptor(serviceType, implemententionType, serviceLifetime));

                wasReplaced = true;
            }

            return wasReplaced;
        }

        public static bool Any<TServiceType>(this IServiceCollection services)
            where TServiceType : class
        {
            return Any(services, typeof(TServiceType));
        }

        public static bool Any(this IServiceCollection services, Type serviceType)
        {
            return services.Any(descriptor => descriptor.ServiceType.Equals(serviceType));
        }
    }
}
