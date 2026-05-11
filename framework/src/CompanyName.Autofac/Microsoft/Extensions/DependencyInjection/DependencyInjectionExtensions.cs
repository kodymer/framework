using Autofac;
using CompanyName.Autofac.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCompanyNameAutofac(this IServiceCollection services)
        {
            services
                .AddCompanyNameCore()
                .AddSingleton<IServiceProviderIsService>(sp =>
                {
                    var lifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                    return new CompanyNameAutofacServiceProviderIsService(lifetimeScope);
                });

            return services;
        }
    }
}
