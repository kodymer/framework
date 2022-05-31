using Microsoft.Extensions.DependencyInjection;
using Vesta.Auditing;
using Vesta.Security.Claims;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaAuditing(this IServiceCollection services)
        {
            services.AddVestaAuditingAbstracts();
            services.AddVestaSecurity();

            services.AddTransient<IAuditPropertySetter, AuditPropertySetter>();

        }
    }
}
