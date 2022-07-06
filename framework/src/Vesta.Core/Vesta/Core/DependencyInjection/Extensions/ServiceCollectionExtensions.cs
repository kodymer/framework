using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LazyProxy.ServiceProvider;

namespace Vesta.Core.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void Replace<TServiceType, TImplemententionType>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            where TServiceType : class
            where TImplemententionType : class, TServiceType
        {
            if (!TryReplace<TServiceType, TImplemententionType>(services, serviceLifetime))
            {
                services.Add(new ServiceDescriptor(typeof(TServiceType), typeof(TImplemententionType), serviceLifetime));
            }
        }

        public static bool TryReplace<TServiceType, TImplemententionType>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            where TServiceType : class
            where TImplemententionType : class, TServiceType
        {
            return TryReplace(services, typeof(TServiceType), typeof(TImplemententionType));
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
