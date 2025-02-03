using LazyProxy.ServiceProvider;
using CompanyName.Auditing;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCompanyNameAuditing(this IServiceCollection services)
        {
            services.AddCompanyNameAuditingAbstracts();
            services.AddCompanyNameSecurity();

            services.AddLazyScoped<IAuditPropertySetter, AuditPropertySetter>();

        }
    }
}
