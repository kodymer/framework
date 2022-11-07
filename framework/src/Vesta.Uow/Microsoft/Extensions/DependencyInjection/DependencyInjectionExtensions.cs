
using LazyProxy.ServiceProvider;
using Vesta.Uow;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaUow(this IServiceCollection services)
        {

            services.AddVestaAutofac();
            services.AddVestaEventBusAbstracts();

            services.AddOptions<UnitOfWorkDefaultOptions>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<UnitOfWorkInterceptor>();

            services.AddLazyScoped<IUnitOfWorkEventPublishingManager, UnitOfWorkEventPublishingManager>();
            services.AddScoped<IUnitOfWorkEventPublishingStore, UnitOfWorkEventPublishingStore>();
        }
    }
}
