using Vesta.EventBus;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaEventBus(this IServiceCollection services)
        {
            services.AddVestaAutofac();
            services.AddVestaEventBusAbstracts();

            services.AddSingleton<IEventHandlerInvoker, EventHandlerInvoker>();
        }

        public static void AddVestaEventHandlers(this IServiceCollection services, Action<EventHandlerOptions> configureOptions)
        {
            var eventHandlerOptions = new EventHandlerOptions();
            configureOptions(eventHandlerOptions);

            services.Configure(configureOptions);

            foreach (var eventHandlerType in eventHandlerOptions.GetAll())
            {
                if (EventBusBase.TryDiscoverEventHandlerInterface(eventHandlerType, out var eventHandlerInterfaceType))
                {
                    services.Add(new ServiceDescriptor(eventHandlerInterfaceType, eventHandlerType, ServiceLifetime.Transient));
                }
            }
        }
    }
}
