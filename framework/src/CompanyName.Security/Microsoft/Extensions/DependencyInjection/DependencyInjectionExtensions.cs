using Microsoft.Extensions.DependencyInjection;
using CompanyName.Security.Claims;
using CompanyName.Security.Users;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCompanyNameSecurity(this IServiceCollection services)
        {
            services.AddCompanyNameCore();

            services.AddSingleton<ICurrentPrincipalAccessor, ThreadCurrentPrincipalAccessor>();
            services.AddTransient<ICurrentUser, CurrentUser>();

        }
    }
}
