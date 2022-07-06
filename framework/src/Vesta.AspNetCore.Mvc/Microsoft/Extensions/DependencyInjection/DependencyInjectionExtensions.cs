using Microsoft.AspNetCore.Mvc;
using Vesta.AspNetCore.Security.Claims;
using Vesta.Core.DependencyInjection.Extensions;
using Vesta.Security.Claims;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaAspNetCoreMvc(this IServiceCollection services, Action<MvcOptions> mvcConfigure = null, Action<JsonOptions> jsonConfigure = null)
        {

            services.AddVestaAspNetCore();

            services.AddHttpContextAccessor();
            services.AddControllers(mvcConfigure)
                .AddControllersAsServices()
                .AddJsonOptions(jsonConfigure);
        }
    }
}
