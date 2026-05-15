using CompanyName.Core;
using CompanyName.EventBus;
using CompanyName.EventBus.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCompanyNameEventBus(this IServiceCollection services)
        {
            services
                .AddCompanyNameAutofac()
                .AddCompanyNameDddDomainEventBus()
                .AddCompanyNameMessagingLocal()
                .AddCompanyNameEventHandlers(Actions.Empty);

            services
                .AddSingleton<DomainEventHandlerTypeProvider>()
                .AddSingleton<IntegrationEventHandlerTypeProvider>()
                .AddSingleton<IEventHandlerInvoker, EventHandlerInvoker>()
                .AddSingleton<ILocalServiceBusMessageConsumer, LocalServiceBusMessageConsumer>();


            Func<IServiceProvider, LocalEventBus> factory = (IServiceProvider serviceProvider) =>
            {
                var eventBus = serviceProvider.GetRequiredService<LocalEventBus>();
                eventBus.Initialize();
                return eventBus;
            };

            services
                .AddSingleton<LocalEventBus>()
                .AddSingleton<ILocalEventBus, LocalEventBus>(factory)
                .AddSingleton<IDistributedEventBus, LocalEventBus>(factory);

            return services;
        }

        public static IServiceCollection AddCompanyNameEventHandlers(this IServiceCollection services, Action<EventHandlerOptions> configureOptions)
        {
            var eventHandlerOptions = new EventHandlerOptions();
            configureOptions(eventHandlerOptions);

            services
                .Configure(configureOptions);

            foreach (var eventHandlerType in eventHandlerOptions.GetAll())
            {
                if (EventHandlerTypeDiscoverer.TryDiscoverEventHandlerInterface(eventHandlerType, out var eventHandlerInterfaceType))
                {
                    services
                        .Add(new ServiceDescriptor(eventHandlerInterfaceType, eventHandlerType, ServiceLifetime.Transient));
                }
            }

            return services;
        }
    }
}
