using CompanyName.Banks.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.Banks.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddBanksDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddCompanyNameDbContext<BanksDbContext>()
                .AddCompanyNameDbContext<TraceabilityDbContext>();

            return services;
        }
    }
}
