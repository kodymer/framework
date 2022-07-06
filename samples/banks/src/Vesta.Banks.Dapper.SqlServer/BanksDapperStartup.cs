using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vesta.Banks.Configuration;

namespace Vesta.Banks.Dapper
{
    public static class BanksDapperStartup
    {
        public static IServiceCollection AddBanksDapper(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddBanksDomain(configuration)
                .AddBanksDatabase(configuration)
                .AddBanksRepositories();

            return services;
        }
    }
}
