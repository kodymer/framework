using Autofac;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.Autofac.Extensions.DependencyInjection
{

    public class CompanyNameAutofacServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        private readonly ContainerBuilder _builder;

        public CompanyNameAutofacServiceProviderFactory(ContainerBuilder builder)
        {
            Guard.IsNotNull(builder);

            _builder = builder;
        }

        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            _builder.Populate(services);

            return _builder;
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            Guard.IsNotNull(containerBuilder);

            return new CompanyNameAutofacServiceProvider(containerBuilder.Build());
        }
    }
}
