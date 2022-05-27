using Vesta.AspNetCore.Security.Claims;
using Vesta.Core.DependencyInjection.Extensions;
using Vesta.Security.Claims;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaAspNetCore(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            if (!services.TryReplace<ICurrentPrincipalAccessor, HttpContextCurrentPrincipalAccessor>())
            {
                services.AddSingleton<ICurrentPrincipalAccessor, HttpContextCurrentPrincipalAccessor>();
            }
        }
    }
}
