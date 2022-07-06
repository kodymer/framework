using Microsoft.Extensions.DependencyInjection;
using Vesta.Banks.EntityFrameworkCore.Repositories;

namespace Vesta.Banks.Configuration
{
    public static class RepositoryConfiguration
    {
        public static IServiceCollection AddBanksRepositories(this IServiceCollection services)
        {
            services.AddTransient<IBankAccountRepository, BankAccountRepository>();

            return services;
        }
    }
}
