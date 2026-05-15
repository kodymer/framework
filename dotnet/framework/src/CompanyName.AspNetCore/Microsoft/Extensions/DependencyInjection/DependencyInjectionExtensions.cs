using CompanyName.AspNetCore.Abstractions.Http;
using CompanyName.AspNetCore.Http;
using CompanyName.AspNetCore.Security.Claims;
using CompanyName.Core.DependencyInjection.Extensions;
using CompanyName.Security.Claims;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCompanyNameAspNetCore(this IServiceCollection services)
        {

            services
                .AddCompanyNameSecurity()
                .AddHttpContextAccessor();

            services
                .AddTransient<IHttpRequestBuilder, DefaultHttpRequestBuilder>()
                .AddTransient<IHttpResponseBuilder, DefaultHttpResponseBuilder>()
                .Replace<ICurrentPrincipalAccessor, HttpContextCurrentPrincipalAccessor>(ServiceLifetime.Singleton);

            return services;
        }
    }
}
