using CompanyName.Core.DependencyInjection.Extensions;
using CompanyName.EventBus.Abstractions;
using CompanyName.EventBus.AzureServiceBus;
using Microsoft.Extensions.Options;
using static Microsoft.Extensions.Options.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {

        public static IServiceCollection AddCompanyNameEventBusAzureServiceBus(this IServiceCollection services, Action<AzureServiceBusEventBusOptions> configureOptions)
        {
            services
                .AddCompanyNameEventBus()
                .AddCompanyNameSecurity()
                .AddCompanyNameMessagingAzureServiceBus();

            services
                .Configure(configureOptions)
                .AddSingleton<AzureServiceBusEventBus>()
                .Replace<IDistributedEventBus, AzureServiceBusEventBus>(serviceProvider =>
                {
                    var eventBus = serviceProvider.GetRequiredService<AzureServiceBusEventBus>();
                    eventBus.Initialize();
                    return eventBus;
                });

            services
                .AddSingleton<IAzureServiceBusMessageConsumer, AzureServiceBusMessageConsumer>();

            return services;
        }

        public static IServiceCollection AddCompanyNameEventBusAzureServiceBus(this IServiceCollection services)
        {
            services
                .AddCompanyNameEventBus()
                .AddCompanyNameSecurity()
                .AddCompanyNameMessagingAzureServiceBus();

            services
                .AddSingleton<IOptionsFactory<AzureServiceBusEventBusOptions>, AzureServiceBusEventBusOptionsFactory>()
                .AddSingleton<IOptions<AzureServiceBusEventBusOptions>>(serviceProvider =>
                {
                    var options = serviceProvider.GetRequiredService<IOptionsFactory<AzureServiceBusEventBusOptions>>().Create(null);
                    return Create(options);
                })
                .AddSingleton<AzureServiceBusEventBus>()
                .AddSingleton<IDistributedEventBus, AzureServiceBusEventBus>(serviceProvider =>
                {
                    var eventBus = serviceProvider.GetRequiredService<AzureServiceBusEventBus>();
                    eventBus.Initialize();
                    return eventBus;
                })
                .AddSingleton<IAzureServiceBusMessageConsumer, AzureServiceBusMessageConsumer>();

            return services;
        }
    }
}
