using Autofac;
using CompanyName.Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseAutofac(this IHostBuilder hostBuilder)
        {
            var containerBuilder = new ContainerBuilder();

            return hostBuilder
                    .ConfigureServices((hostBuilderContext, services) => services.AddCompanyNameAutofac())
                    .UseServiceProviderFactory(new CompanyNameAutofacServiceProviderFactory(containerBuilder));
        }
    }
}