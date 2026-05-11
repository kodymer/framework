using CompanyName.Data;
using CompanyName.EntityFrameworkCore;
using CompanyName.EntityFrameworkCore.Abstractions;
using CompanyName.Uow;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityFrameworkServiceCollectionExtensions
    {
        public static IServiceCollection AddCompanyNameDbContext<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null , ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            where TContext : CompanyNameDbContextBase<TContext>
        {
            services
                .AddCompanyNameUow()
                .AddCompanyNameEntityFrameworkCore();

            (
                optionsAction is not null ?
                    services.AddDbContext<TContext>(optionsAction, contextLifetime, optionsLifetime) :
                    services.AddDbContext<TContext>(contextLifetime, optionsLifetime)
            )
            .AddCompanyNameDbContextProvider<TContext>();

            return services;
        }

        public static IServiceCollection AddCompanyNameDbContext<TContext>(this IServiceCollection services, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            where TContext : CompanyNameDbContextBase<TContext>
        {
            services
                .AddCompanyNameUow()
                .AddCompanyNameEntityFrameworkCore();

                (
                    optionsAction is not null ?
                        services.AddDbContext<TContext>(optionsAction, contextLifetime, optionsLifetime) :
                        services.AddDbContext<TContext>(contextLifetime, optionsLifetime)
                )
                .AddCompanyNameDbContextProvider<TContext>();

            return services;
        }

        public static IServiceCollection AddCompanyNameDbContext<TContext>(this IServiceCollection services, ServiceLifetime contextLifetime, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
           where TContext : CompanyNameDbContextBase<TContext>
        {
            services
                .AddCompanyNameUow()
                .AddCompanyNameEntityFrameworkCore();


             services
                .AddDbContext<TContext>(contextLifetime, optionsLifetime)
                .AddCompanyNameDbContextProvider<TContext>();

            return services;
        }

        internal static IServiceCollection AddCompanyNameDbContextProvider<TContext>(this IServiceCollection services)
            where TContext : CompanyNameDbContextBase<TContext>
        {
            services
                .AddScoped<IDbContextProvider<TContext>, DbContextProvider<TContext>>()
                .AddScoped<IUnitOfWorkApiFactory<TContext>, UnitOfWorkApiFactory<TContext>>();

            return services;
        }

    }
}