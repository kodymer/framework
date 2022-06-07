using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vesta.Banks.Configuration;

namespace Vesta.Banks.EntityFrameworkCore
{
    public static class BanksEntityFrameworkCoreStartup
    {
        public static void AddBanksEntityFrameworkCore(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddBanksDbContext(configuration)
                .AddBanksRepositories();
        }
    }
}
