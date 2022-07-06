
using Microsoft.Extensions.Azure;
using Vesta.ServiceBus.Azure;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaSeviceBusAzure(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionPool, ConnectionPool>();
            services.AddSingleton<IProcessorPool, ProcessorPool>();
            services.AddSingleton<IPublisherPool, PublisherPool>();
        }
    }
}
