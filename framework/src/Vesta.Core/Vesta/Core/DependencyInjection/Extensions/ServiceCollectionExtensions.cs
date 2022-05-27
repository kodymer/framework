using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

            if (services.Any(descriptor => descriptor.ServiceType.Equals(typeof(TServiceType))))
            {
                services.Replace(
                    new ServiceDescriptor(typeof(TServiceType), typeof(TImplemententionType), ServiceLifetime.Singleton));

                wasReplaced = true;
            }

            return wasReplaced;
        }
    }
}
