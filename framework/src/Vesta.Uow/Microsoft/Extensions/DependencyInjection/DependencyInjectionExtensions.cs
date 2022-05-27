
using LazyProxy.ServiceProvider;
using Vesta.Uow;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaUow(this IServiceCollection services)
        {
            services.AddLazyScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
