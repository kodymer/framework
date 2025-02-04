using LazyProxy.ServiceProvider;
using CompanyName.ServiceBus.Local;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCompanyNameServiceBusLocal(this IServiceCollection services)
        {
            services.AddSingleton<LocalServiceBusQueue>();
            services.AddSingleton<ILocalServiceBusSender, LocalServiceBusSender>();
            services.AddSingleton<ILocalServiceBusProcessor, LocalServiceBusProcessor>();
        }
    }
}
