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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using static Microsoft.Extensions.Options.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {

        public static void AddVestaEventBusAzure(this IServiceCollection services, Action<AzureEventBusOptions> configureOptions)
        {
            services.AddVestaEventBus();
            services.AddVestaSecurity();
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

        public static void AddVestaEventBusAzure(this IServiceCollection services)
        {
            services.AddVestaEventBus();
            services.AddVestaSecurity();
            services.AddVestaSeviceBusAzure();

            services.AddSingleton<IOptionsFactory<AzureEventBusOptions>, AzureEventBusOptionsFactory>();
            services.AddSingleton<IOptions<AzureEventBusOptions>>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptionsFactory<AzureEventBusOptions>>().Create(null);
                return Create(options);
            });

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
