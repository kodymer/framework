
using LazyProxy.ServiceProvider;
using CompanyName.Uow;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCompanyNameUow(this IServiceCollection services)
        {

            services.AddCompanyNameAutofac();
            services.AddCompanyNameEventBusAbstracts();

            services.AddOptions<UnitOfWorkDefaultOptions>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<UnitOfWorkInterceptor>();

            services.AddLazyScoped<IUnitOfWorkEventPublishingManager, UnitOfWorkEventPublishingManager>();
            services.AddScoped<IUnitOfWorkEventPublishingStore, UnitOfWorkEventPublishingStore>();
        }
    }
}
