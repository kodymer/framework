using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace CompanyName.Autofac.Extensions.DependencyInjection
{
    public class CompanyNameAutofacServiceProvider : AutofacServiceProvider
    {
        public CompanyNameAutofacServiceProvider(ILifetimeScope lifetimeScope)
            : base(lifetimeScope)
        {
        }
    }
}
