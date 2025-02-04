using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CompanyName.Banks.EntityFrameworkCore;

namespace CompanyName.Banks.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddBanksDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCompanyNameDbContext<BanksDbContext>();
            services.AddCompanyNameDbContext<TraceabilityDbContext>();

            return services;
        }
    }
}
