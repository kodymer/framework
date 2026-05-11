using CompanyName.Security.Claims;
using CompanyName.Security.Users;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using static Microsoft.Extensions.Options.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCompanyNameSecurity(this IServiceCollection services)
        {

            services
                .AddCompanyNameCore()
                .AddSingleton<ICurrentPrincipalAccessor, ThreadCurrentPrincipalAccessor>()
                .AddTransient<ICurrentUser, CurrentUser>()
                .AddOptions<ClaimTypeOptions>();

            return services;
        }

        public static IServiceCollection AddCompanyNameClaims(this IServiceCollection services, Action<ClaimTypeOptions> configureOptions)
        {
            services
                .AddCompanyNameSecurity();

            if (configureOptions is not null)
            {
                var options = new ClaimTypeOptions();
                configureOptions(options);
                
                services
                    .Replace(new ServiceDescriptor(typeof(IOptions<ClaimTypeOptions>), Create(options)));
            }

            return services;
        }
    }
}
