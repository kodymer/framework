
using Microsoft.Extensions.Azure;
using CompanyName.ServiceBus.Azure;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCompanyNameSeviceBusAzure(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionPool, ConnectionPool>();
            services.AddSingleton<IProcessorPool, ProcessorPool>();
            services.AddSingleton<IPublisherPool, PublisherPool>();
        }
    }
}
