using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vesta.Banks.EntityFrameworkCore;

namespace Vesta.Banks.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddBanksDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddVestaDbContext<BanksDbContext>();

            return services;
        }
    }
}
