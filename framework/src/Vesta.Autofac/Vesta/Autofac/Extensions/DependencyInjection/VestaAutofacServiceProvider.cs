using Autofac;
using Autofac.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.Autofac.Extensions.DependencyInjection
{
    public class VestaAutofacServiceProvider : AutofacServiceProvider
    {
        public VestaAutofacServiceProvider(ILifetimeScope lifetimeScope)
            : base(lifetimeScope)
        {
        }
    }
}
