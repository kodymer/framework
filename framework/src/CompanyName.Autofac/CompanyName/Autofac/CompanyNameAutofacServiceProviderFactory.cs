using Ardalis.GuardClauses;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using CompanyName.Autofac.Extensions.DependencyInjection;

namespace CompanyName.Autofac
{

    public class CompanyNameAutofacServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        private readonly ContainerBuilder _builder;
        private IServiceCollection _services;

        public CompanyNameAutofacServiceProviderFactory(ContainerBuilder builder)
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

            return new CompanyNameAutofacServiceProvider(containerBuilder.Build());
        }
    }
}
