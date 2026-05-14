using CompanyName.Banks.Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.Banks.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddBanksDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCompanyNameDatabase<BanksDatabase>();

            return services;
        }
    }
}
