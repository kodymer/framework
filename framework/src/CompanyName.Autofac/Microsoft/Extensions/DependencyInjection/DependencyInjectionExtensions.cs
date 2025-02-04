using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyName.Core.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCompanyNameAutofac(this IServiceCollection services)
        {
            services.AddCompanyNameCore();
        }
    }
}
