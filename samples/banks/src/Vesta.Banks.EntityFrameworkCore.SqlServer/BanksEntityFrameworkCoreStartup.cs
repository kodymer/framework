using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vesta.Banks.Configuration;

namespace Vesta.Banks.EntityFrameworkCore
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
