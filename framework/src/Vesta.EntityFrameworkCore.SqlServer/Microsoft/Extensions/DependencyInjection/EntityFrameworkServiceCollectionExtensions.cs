using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Vesta.Data;
using Vesta.EntityFrameworkCore;
using Vesta.EntityFrameworkCore.Abstracts;
using Vesta.Uow.EntityFrameworkCore.SqlServer;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityFrameworkServiceCollectionExtensions
    {
        public static void AddVestaDbContext<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext, IEfCoreDbContext
        {
            services.AddVestaUow();
            services.AddVestaEntityFrameworkCore();

            services.AddDbContext<TDbContext>(optionsAction, serviceLifetime);
            services.AddTransient<IDbContextProvider<TDbContext>, SqlServerDbContextProvider<TDbContext>>();
        }


        public static void AddVestaDbContext<TDbContext>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext, IEfCoreDbContext
        {
            services.AddVestaUow();
            services.AddVestaEntityFrameworkCore();

            services.AddDbContext<TDbContext>((serviceProvider, optionsAction) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                optionsAction.UseSqlServer(configuration.GetConnectionString(ConnectionStrings.DefaultNameConfig));

            }, serviceLifetime);

            services.AddTransient<IDbContextProvider<TDbContext>, SqlServerDbContextProvider<TDbContext>>();
        }

    }
}
