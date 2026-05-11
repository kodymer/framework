using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.Autofac.Extensions.DependencyInjection
{
    public class CompanyNameAutofacServiceProviderIsService : IServiceProviderIsService
    {
        private readonly ILifetimeScope _lifetimeScope;

        public CompanyNameAutofacServiceProviderIsService(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public bool IsService(Type serviceType)
        {
            return _lifetimeScope.IsRegistered(serviceType);
        }
    }
}