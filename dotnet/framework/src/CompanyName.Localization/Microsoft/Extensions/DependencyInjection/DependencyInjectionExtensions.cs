using CompanyName.Localization;
using CompanyName.Localization.Resources;
using LazyProxy;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCompanyNameLocalization(this IServiceCollection services, Action<CompanyNameLocalizationOptions> setupAction)
        {
            var localizationOptions = new CompanyNameLocalizationOptions();
            setupAction(localizationOptions);

            services
                .AddCompanyNameCore()
                .Configure(setupAction)
                .AddLocalization(options => options.ResourcesPath = localizationOptions.ResourcesPath)
                .AddTransient<ResourceManagerStringLocalizerFactory>()
                .Replace(new ServiceDescriptor(typeof(IStringLocalizerFactory), typeof(CompanyNameResourceManagerStringLocalizerFactory), ServiceLifetime.Singleton));

            /* 
             * Lazy instance
             * 
             * var stringLocalizerFactoryServiceDescriptor = new ServiceDescriptor(
             *  typeof(IStringLocalizerFactory),
             *  (IServiceProvider serviceProvider) => LazyProxyBuilder.CreateInstance(
             *      typeof(IStringLocalizerFactory), 
             *      () => serviceProvider.GetService<CompanyNameResourceManagerStringLocalizerFactory>()));
             *
             */

            return services;
        }

        public static IServiceCollection AddCompanyNameLocalization(this IServiceCollection services)
        {

            services.
                 AddCompanyNameLocalization(options => options.DefaultResourceType = typeof(DefaultResource));

            return services;
        }
    }
}
