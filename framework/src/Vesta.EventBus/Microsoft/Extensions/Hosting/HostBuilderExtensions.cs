using Microsoft.Extensions.DependencyInjection;
using Vesta.EventBus.Hosting;

namespace Microsoft.Extensions.Hosting
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseEventBus(this IHostBuilder hostBuilder)
        {

            return hostBuilder
                    .ConfigureServices((hostBuilderContext, services) => services.AddHostedService<EventBusInitializer>());
        }
    }
}