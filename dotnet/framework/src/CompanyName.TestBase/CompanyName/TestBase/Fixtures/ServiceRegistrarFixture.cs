using Autofac;
using CompanyName.Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.TestBase.Fixtures
{
    public class ServiceRegistrarFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public ServiceRegistrarFixture()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var containerBuilder = new ContainerBuilder();
            var serviceProviderFactory = new CompanyNameAutofacServiceProviderFactory(containerBuilder);

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
