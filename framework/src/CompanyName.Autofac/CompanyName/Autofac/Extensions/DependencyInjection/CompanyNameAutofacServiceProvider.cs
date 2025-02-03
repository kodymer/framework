using Autofac;
using Autofac.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
