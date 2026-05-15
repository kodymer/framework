using CompanyName.Data;
using CompanyName.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityFrameworkServiceCollectionExtensions
    {
        public static IServiceCollection AddCompanyNameDbContext<TContext>(this IServiceCollection services, string connectionStringName = ConnectionStrings.DefaultNameConfig, Action<SqlServerDbContextOptionsBuilder> sqlServerOptionAction = null)
            where TContext : CompanyNameDbContextBase<TContext>
        {
            services
                .AddCompanyNameUow()
                .AddCompanyNameEntityFrameworkCore();

            services
                .AddDbContext<TContext>((serviceProvider, options) =>
                {
                    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                    options.UseSqlServer(configuration.GetConnectionString(connectionStringName), sqlServerOptionAction);
                })
                .AddCompanyNameDbContextProvider<TContext>();

            return services;
        }
    }
}
