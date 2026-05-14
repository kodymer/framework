using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Samples.Banks.Dapper;

namespace Samples.Banks
{
    public static class BanksDapper
    {
        public static IServiceCollection AddBanksDapper(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddCompanyNameDatabase<BanksDatabase>()
                .AddBanksRepositories();

            services
                .AddBanksApplication(configuration);

            return services;
        }
    }
}
