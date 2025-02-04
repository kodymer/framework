using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CompanyName.Banks.Configuration;

namespace CompanyName.Banks.EntityFrameworkCore
{
    public static class BanksEntityFrameworkCoreStartup
    {
        public static IServiceCollection AddBanksEntityFrameworkCore(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddBanksDomain(configuration)
                .AddBanksDbContext(configuration)
                .AddBanksRepositories();

            return services;
        }
    }
}
