using Microsoft.AspNetCore.Mvc;
using CompanyName.AspNetCore.Security.Claims;
using CompanyName.Core.DependencyInjection.Extensions;
using CompanyName.Security.Claims;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCompanyNameAspNetCoreMvc(this IServiceCollection services, Action<MvcOptions> mvcConfigure = null, Action<JsonOptions> jsonConfigure = null)
        {

            services.AddCompanyNameAspNetCore();

            services.AddHttpContextAccessor();
            services.AddControllers(mvcConfigure)
                .AddControllersAsServices()
                .AddJsonOptions(jsonConfigure);
        }
    }
}
