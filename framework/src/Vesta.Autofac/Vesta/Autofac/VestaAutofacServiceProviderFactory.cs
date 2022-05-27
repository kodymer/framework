using Ardalis.GuardClauses;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Vesta.Autofac.Extensions.DependencyInjection;

namespace Vesta.Autofac
{

    public class VestaAutofacServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        private readonly ContainerBuilder _builder;
        private IServiceCollection _services;

        public VestaAutofacServiceProviderFactory(ContainerBuilder builder)
        {
            _builder = builder;
        }

        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            _services = services;

            _builder.Populate(services);

            return _builder;
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            Guard.Against.Null(containerBuilder, nameof(containerBuilder));

            return new VestaAutofacServiceProvider(containerBuilder.Build());
        }
    }
}
