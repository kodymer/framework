using LazyProxy.ServiceProvider;
using Vesta.Auditing;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaAuditing(this IServiceCollection services)
        {
            services.AddVestaAuditingAbstracts();
            services.AddVestaSecurity();

            services.AddLazyScoped<IAuditPropertySetter, AuditPropertySetter>();

        }
    }
}
