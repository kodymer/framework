using Microsoft.Extensions.DependencyInjection;
using Vesta.Security.Claims;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaSecurity(this IServiceCollection services)
        {
            services.AddVestaCore();

            services.AddSingleton<ICurrentPrincipalAccessor, ThreadCurrentPrincipalAccessor>();

        }
    }
}
