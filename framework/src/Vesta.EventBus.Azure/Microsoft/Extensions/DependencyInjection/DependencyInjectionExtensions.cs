using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.Core.DependencyInjection.Extensions;
using Vesta.EventBus;
using Vesta.EventBus.Azure;
using LazyProxy.ServiceProvider;
using Vesta.EventBus.Abstracts;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaEventBusAzure(this IServiceCollection services, Action<AzureEventBusOptions> configureOptions)
        {
            services.AddVestaEventBus();
            services.AddVestaSeviceBusAzure();

            services.Configure(configureOptions);

            services.AddSingleton<AzureEventBus>();
            services.AddSingleton<IDistributedEventBus, AzureEventBus>(serviceProvider =>
            {
                var eventBus = serviceProvider.GetRequiredService<AzureEventBus>();
                eventBus.Initialize();
                return eventBus;
            });

            services.AddSingleton<IAzureServiceBusMessageConsumer, AzureServiceBusMessageConsumer>();

        }
    }
}
