using Vesta.AspNetCore.Security.Claims;
using Vesta.Core.DependencyInjection.Extensions;
using Vesta.Security.Claims;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaAspNetCore(this IServiceCollection services)
        {

            services.AddVestaSecurity();

            services.AddHttpContextAccessor();

            services.Replace<ICurrentPrincipalAccessor, HttpContextCurrentPrincipalAccessor>(ServiceLifetime.Singleton);
        }
    }
}
