using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Vesta.Autofac;

namespace Microsoft.Extensions.Hosting
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseAutofac(this IHostBuilder hostBuilder)
        {
            var containerBuilder = new ContainerBuilder();

            return hostBuilder
                    .ConfigureServices((hostBuilderContext, services) => services.AddVestaAutofac())
                    .UseServiceProviderFactory(new VestaAutofacServiceProviderFactory(containerBuilder));
        }
    }
}