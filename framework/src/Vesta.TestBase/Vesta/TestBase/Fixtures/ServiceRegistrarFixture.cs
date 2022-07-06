using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Vesta.Autofac;

namespace Vesta.TestBase.Fixtures
{
    public class ServiceRegistrarFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public ServiceRegistrarFixture()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var containerBuilder = new ContainerBuilder();
            var serviceProviderFactory = new VestaAutofacServiceProviderFactory(containerBuilder);

            serviceProviderFactory.CreateBuilder(services);

            ServiceProvider = serviceProviderFactory.CreateServiceProvider(containerBuilder);
        }

        public virtual void ConfigureServices(ServiceCollection services)
        {
            // Override for registrar services
        }


        public void Dispose()
        {
            ServiceProvider.As<IDisposable>().Dispose();
        }
    }
}
