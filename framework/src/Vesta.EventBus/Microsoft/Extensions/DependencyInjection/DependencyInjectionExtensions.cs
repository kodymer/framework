using Vesta.EventBus;
using Vesta.EventBus.Abstracts;
using Vesta.EventBus.Azure;
using Vesta.ServiceBus.Local;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaEventBus(this IServiceCollection services)
        {
            services.AddVestaAutofac();
            services.AddVestaEventBusAbstracts();
            services.AddVestaDddDomainEventBus();
            services.AddVestaServiceBusLocal();

            services.AddSingleton<DomainEventHandlerTypeProvider>();
            services.AddSingleton<IntegrationEventHandlerTypeProvider>();
            services.AddSingleton<IEventHandlerInvoker, EventHandlerInvoker>();
            services.AddSingleton<ILocalServiceBusMessageConsumer, LocalServiceBusMessageConsumer>();

            services.AddSingleton<LocalEventBus>();
            services.AddSingleton<ILocalEventBus, LocalEventBus>(serviceProvider =>
            {
                var eventBus = serviceProvider.GetRequiredService<LocalEventBus>();
                eventBus.Initialize();
                return eventBus;
            });
        }

        public static void AddVestaEventHandlers(this IServiceCollection services, Action<EventHandlerOptions> configureOptions)
        {
            var eventHandlerOptions = new EventHandlerOptions();
            configureOptions(eventHandlerOptions);

            services.Configure(configureOptions);

            foreach (var eventHandlerType in eventHandlerOptions.GetAll())
            {
                if (EventHandlerTypeDiscoverer.TryDiscoverEventHandlerInterface(eventHandlerType, out var eventHandlerInterfaceType))
                {
                    services.Add(new ServiceDescriptor(eventHandlerInterfaceType, eventHandlerType, ServiceLifetime.Transient));
                }
            }
        }
    }
}
