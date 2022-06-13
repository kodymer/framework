﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.Core.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaDddDomain(this IServiceCollection services)
        {
            services.AddVestaAuditingAbstracts();             
            services.AddVestaDddDomainEventBus();             
            services.AddVestaSecurity();             
        }
    }
}
