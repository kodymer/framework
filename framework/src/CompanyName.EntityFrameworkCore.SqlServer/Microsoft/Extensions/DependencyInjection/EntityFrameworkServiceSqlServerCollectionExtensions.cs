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
        public static void AddCompanyNameDbContext<TDbContext>(this IServiceCollection services, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext, IEfCoreDbContext
        {
            services.AddCompanyNameUow();
            services.AddCompanyNameEntityFrameworkCore();

            services.AddCompanyNameDbContext<TDbContext>((serviceProvider, optionsAction) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                optionsAction.UseSqlServer(configuration.GetConnectionString(ConnectionStrings.DefaultNameConfig));

            }, contextLifetime, optionsLifetime);
        }

    }
}
