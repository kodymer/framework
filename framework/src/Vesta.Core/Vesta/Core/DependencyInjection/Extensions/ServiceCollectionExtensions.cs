using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.Core.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static bool TryReplace<TServiceType, TImplemententionType>(this IServiceCollection services)
            where TServiceType : class
            where TImplemententionType : class, TServiceType
        {
            bool wasReplaced = false;

            if (Any<TServiceType>(services))
            {
                services.Replace(
                    new ServiceDescriptor(typeof(TServiceType), typeof(TImplemententionType), ServiceLifetime.Singleton));

                wasReplaced = true;
            }

            return wasReplaced;
        }

        public static bool Any<TServiceType>(this IServiceCollection services)
            where TServiceType : class
        {
            return services.Any(descriptor => descriptor.ServiceType.Equals(typeof(TServiceType)));
        }
    }
}
