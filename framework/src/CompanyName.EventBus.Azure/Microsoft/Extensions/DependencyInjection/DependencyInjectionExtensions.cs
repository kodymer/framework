using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyName.Core.DependencyInjection.Extensions;
using CompanyName.EventBus;
using CompanyName.EventBus.Azure;
using LazyProxy.ServiceProvider;
using CompanyName.EventBus.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using static Microsoft.Extensions.Options.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {

        public static IServiceCollection AddCompanyNameEventBusAzure(this IServiceCollection services, Action<AzureEventBusOptions> configureOptions)
        {
            services
                .AddCompanyNameEventBus()
                .AddCompanyNameSecurity()
                .AddCompanyNameSeviceBusAzure();

            services
                .Configure(configureOptions)
                .AddSingleton<AzureEventBus>()
                .AddSingleton<IDistributedEventBus, AzureEventBus>(serviceProvider =>
            {
                var eventBus = serviceProvider.GetRequiredService<AzureEventBus>();
                eventBus.Initialize();
                return eventBus;
            });

            services
                .AddSingleton<IAzureServiceBusMessageConsumer, AzureServiceBusMessageConsumer>();

            return services;
        }

        public static void AddCompanyNameEventBusAzure(this IServiceCollection services)
        {
            services.AddCompanyNameEventBus();
            services.AddCompanyNameSecurity();
            services.AddCompanyNameSeviceBusAzure();

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
