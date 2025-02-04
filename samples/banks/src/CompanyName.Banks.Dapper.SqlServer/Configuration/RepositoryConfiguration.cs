using Microsoft.Extensions.DependencyInjection;
using CompanyName.Banks.Dapper.Repositories;

namespace CompanyName.Banks.Configuration
{
    public static class RepositoryConfiguration
    {
        public static IServiceCollection AddBanksRepositories(this IServiceCollection services)
        {
            services.AddTransient<IBankTransferRepository, BankTransferRepository>();

            return services;
        }
    }
}
