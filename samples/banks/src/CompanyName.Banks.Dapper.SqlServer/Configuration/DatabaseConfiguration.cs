using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vesta.Banks.Dapper;
using Vesta.Dapper.SqlServer;

namespace Vesta.Banks.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddBanksDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddVestaDatabase<BanksDatabase>();

            return services;
        }
    }
}
