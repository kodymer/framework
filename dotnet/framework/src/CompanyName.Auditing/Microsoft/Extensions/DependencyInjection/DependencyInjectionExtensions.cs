using LazyProxy.ServiceProvider;
using CompanyName.Auditing;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCompanyNameAuditing(this IServiceCollection services)
        {
            services
                .AddCompanyNameAuditingAbstractions()
                .AddCompanyNameSecurity()
                .AddLazyScoped<IAuditPropertySetter, AuditPropertySetter>();

            return services;
        }
    }
}
