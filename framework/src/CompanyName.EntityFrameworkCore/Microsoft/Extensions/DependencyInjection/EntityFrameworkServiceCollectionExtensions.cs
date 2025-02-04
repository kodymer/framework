using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CompanyName.Data;
using CompanyName.EntityFrameworkCore.Abstracts;
using CompanyName.Uow;
using CompanyName.Uow.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityFrameworkServiceCollectionExtensions
    {
        public static void AddCompanyNameDbContext<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext, IEfCoreDbContext
        {
            services.AddCompanyNameUow();
            services.AddCompanyNameEntityFrameworkCore();

            services.AddDbContext<TDbContext>(optionsAction, contextLifetime, optionsLifetime);
            services.AddScoped<IDbContextProvider<TDbContext>, EfCoreDbContextProvider<TDbContext>>();
            services.AddScoped<IUnitOfWorkApiFactory<TDbContext>, EfCoreUnitOfWorkApiFactory<TDbContext>>();
        }

        public static void AddCompanyNameDbContext<TDbContext>(this IServiceCollection services, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext, IEfCoreDbContext
        {
            services.AddCompanyNameUow();
            services.AddCompanyNameEntityFrameworkCore();

            services.AddDbContext<TDbContext>(optionsAction, contextLifetime, optionsLifetime);
            services.AddScoped<IDbContextProvider<TDbContext>, EfCoreDbContextProvider<TDbContext>>();
            services.AddScoped<IUnitOfWorkApiFactory<TDbContext>, EfCoreUnitOfWorkApiFactory<TDbContext>>();
        }
    }
}
