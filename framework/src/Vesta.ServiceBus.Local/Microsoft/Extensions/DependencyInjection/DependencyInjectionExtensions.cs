using LazyProxy.ServiceProvider;
using Vesta.ServiceBus.Local;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaServiceBusLocal(this IServiceCollection services)
        {
            services.AddSingleton<LocalServiceBusQueue>();
            services.AddSingleton<ILocalServiceBusSender, LocalServiceBusSender>();
            services.AddSingleton<ILocalServiceBusProcessor, LocalServiceBusProcessor>();
        }
    }
}
