using LazyProxy.ServiceProvider;
using Vesta.Uow;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaEntityFrameworkCore(this IServiceCollection services)
        {
            services.AddVestaEntityFrameworkCoreAbstracts();
            services.AddVestaAuditing();
            services.AddVestaDddDomain();
            services.AddVestaUow();
            services.AddVestaData();

            services.AddLazyScoped<IUnitOfWorkEventRecordRegistrar, UnitOfWorkEventRecordRegistrar>();
            services.AddLazyScoped<IUnitOfWorkEventRecordRegistrar, UnitOfWorkEventRecordRegistrar>();
        }
    }
}
