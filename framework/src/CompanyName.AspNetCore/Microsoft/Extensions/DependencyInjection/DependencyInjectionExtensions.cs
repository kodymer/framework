using CompanyName.AspNetCore.Security.Claims;
using CompanyName.Core.DependencyInjection.Extensions;
using CompanyName.Security.Claims;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCompanyNameAspNetCore(this IServiceCollection services)
        {

            services.AddCompanyNameSecurity();

            services.AddHttpContextAccessor();

            services.Replace<ICurrentPrincipalAccessor, HttpContextCurrentPrincipalAccessor>(ServiceLifetime.Singleton);
        }
    }
}
