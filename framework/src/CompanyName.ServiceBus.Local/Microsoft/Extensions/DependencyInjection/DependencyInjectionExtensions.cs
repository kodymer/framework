using LazyProxy.ServiceProvider;
using CompanyName.ServiceBus.Local;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCompanyNameServiceBusLocal(this IServiceCollection services)
        {
            services
                .AddSingleton<LocalServiceBusQueue>()
                .AddSingleton<ILocalServiceBusSender, LocalServiceBusSender>()
                .AddSingleton<ILocalServiceBusProcessor, LocalServiceBusProcessor>();

            return services;
        }
    }
}
