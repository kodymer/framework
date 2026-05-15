using LazyProxy.ServiceProvider;
using CompanyName.Messaging.InProcess;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCompanyNameMessagingLocal(this IServiceCollection services)
        {
            services
                .AddSingleton<LocalServiceBusQueue>()
                .AddSingleton<ILocalServiceBusSender, LocalServiceBusSender>()
                .AddSingleton<ILocalServiceBusProcessor, LocalServiceBusProcessor>();

            return services;
        }
    }
}
