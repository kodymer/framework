﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaEntityFrameworkCoreAbstracts(this IServiceCollection services)
        {
            services.AddVestaCore();
        }
    }
}
