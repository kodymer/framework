
using LazyProxy.ServiceProvider;
using Vesta.Uow;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaUow(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddLazyScoped<IUnitOfWorkEventPublishingManager, UnitOfWorkEventPublishingManager>();
            services.AddScoped<IUnitOfWorkEventPublishingStore, UnitOfWorkEventPublishingStore>();
        }
    }
}
