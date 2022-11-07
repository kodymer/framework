using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Vesta.Data;
using Vesta.EntityFrameworkCore.Abstracts;
using Vesta.Uow;
using Vesta.Uow.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityFrameworkServiceCollectionExtensions
    {
        public static void AddVestaDbContext<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext, IEfCoreDbContext
        {
            services.AddVestaUow();
            services.AddVestaEntityFrameworkCore();

            services.AddDbContext<TDbContext>(optionsAction, contextLifetime, optionsLifetime);
            services.AddScoped<IDbContextProvider<TDbContext>, EfCoreDbContextProvider<TDbContext>>();
            services.AddScoped<IUnitOfWorkApiFactory<TDbContext>, EfCoreUnitOfWorkApiFactory<TDbContext>>();
        }

        public static void AddVestaDbContext<TDbContext>(this IServiceCollection services, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext, IEfCoreDbContext
        {
            services.AddVestaUow();
            services.AddVestaEntityFrameworkCore();

            services.AddDbContext<TDbContext>(optionsAction, contextLifetime, optionsLifetime);
            services.AddScoped<IDbContextProvider<TDbContext>, EfCoreDbContextProvider<TDbContext>>();
            services.AddScoped<IUnitOfWorkApiFactory<TDbContext>, EfCoreUnitOfWorkApiFactory<TDbContext>>();
        }
    }
}
