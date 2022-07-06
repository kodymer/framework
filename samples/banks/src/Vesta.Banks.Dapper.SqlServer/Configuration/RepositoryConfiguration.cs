using Microsoft.Extensions.DependencyInjection;
using Vesta.Banks.Dapper.Repositories;

namespace Vesta.Banks.Configuration
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
