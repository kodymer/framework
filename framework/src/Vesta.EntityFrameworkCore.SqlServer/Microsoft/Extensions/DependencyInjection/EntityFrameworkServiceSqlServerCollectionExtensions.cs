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
        public static void AddVestaDbContext<TDbContext>(this IServiceCollection services, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext, IEfCoreDbContext
        {
            services.AddVestaUow();
            services.AddVestaEntityFrameworkCore();

            services.AddVestaDbContext<TDbContext>((serviceProvider, optionsAction) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                optionsAction.UseSqlServer(configuration.GetConnectionString(ConnectionStrings.DefaultNameConfig));

            }, contextLifetime, optionsLifetime);
        }

    }
}
