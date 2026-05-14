using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Samples.Banks.EntityFrameworkCore;

namespace Samples.Banks
{
    public static class BanksEntityFrameworkCore
    {
        public static IServiceCollection AddBanksEntityFrameworkCore(this IServiceCollection services, IConfiguration configuration)
        {

            services
                .AddCompanyNameDbContext<BankDbContext>(ServiceLifetime.Scoped)
                .AddCompanyNameDbContext<TraceabilityDbContext>(ServiceLifetime.Scoped);

            services
                .AddBanksApplication(configuration)
                .AddBanksRepositories();

            return services;
        }
    }
}
