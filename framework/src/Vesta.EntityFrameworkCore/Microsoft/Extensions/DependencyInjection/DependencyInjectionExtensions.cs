using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaEntityFrameworkCore(this IServiceCollection services)
        {
            services.AddVestaEntityFrameworkCoreAbstracts();
            services.AddVestaAuditing();
            services.AddVestaDddDomain();
            services.AddVestaUow();
        }
    }
}
