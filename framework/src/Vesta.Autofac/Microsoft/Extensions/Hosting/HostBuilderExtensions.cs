using Autofac;
using Vesta.Autofac;

namespace Microsoft.Extensions.Hosting
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseAutofac(this IHostBuilder hostBuilder)
        {
            var containerBuilder = new ContainerBuilder();

            return hostBuilder
                    .UseServiceProviderFactory(new VestaAutofacServiceProviderFactory(containerBuilder));
        }
    }
}