using CompanyName.Core;
using CompanyName.EventBus;
using CompanyName.EventBus.Abstracts;
using CompanyName.EventBus.Azure;
using CompanyName.ServiceBus.Local;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCompanyNameEventBus(this IServiceCollection services)
        {
            services.AddCompanyNameAutofac();
            services.AddCompanyNameEventBusAbstracts();
            services.AddCompanyNameDddDomainEventBus();
            services.AddCompanyNameServiceBusLocal();
            services.AddCompanyNameEventHandlers(Actions.Empty);

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

        public static void AddCompanyNameEventHandlers(this IServiceCollection services, Action<EventHandlerOptions> configureOptions)
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
