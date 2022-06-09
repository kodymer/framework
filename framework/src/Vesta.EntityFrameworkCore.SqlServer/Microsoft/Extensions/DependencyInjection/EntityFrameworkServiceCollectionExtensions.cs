using Microsoft.EntityFrameworkCore;
using Vesta.EntityFrameworkCore;
using Vesta.EntityFrameworkCore.Abstracts;
using Vesta.Uow.EntityFrameworkCore.SqlServer;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityFrameworkServiceCollectionExtensions
    {
        public static void AddVestaDbContext<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext, IEfCoreDbContext
        {
            services.AddVestaUow();
            services.AddVestaEntityFrameworkCore();

            services.AddDbContext<TDbContext>(optionsAction, serviceLifetime);
            services.AddTransient<IDbContextProvider<TDbContext>, SqlServerDbContextProvider<TDbContext>>();
        }
    }
}
