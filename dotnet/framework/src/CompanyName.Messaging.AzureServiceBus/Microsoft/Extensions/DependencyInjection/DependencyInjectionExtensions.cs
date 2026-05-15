
using Microsoft.Extensions.Azure;
using CompanyName.Messaging.AzureServiceBus;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCompanyNameMessagingAzureServiceBus(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionPool, ConnectionPool>();
            services.AddSingleton<IProcessorPool, ProcessorPool>();
            services.AddSingleton<IPublisherPool, PublisherPool>();
        }
    }
}
