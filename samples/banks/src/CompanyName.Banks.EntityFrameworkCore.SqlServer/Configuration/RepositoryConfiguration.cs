using Microsoft.Extensions.DependencyInjection;
using Vesta.Banks.EntityFrameworkCore.Repositories;
using Vesta.Banks.Traceability;

namespace Vesta.Banks.Configuration
{
    public static class RepositoryConfiguration
    {
        public static IServiceCollection AddBanksRepositories(this IServiceCollection services)
        {
            services.AddTransient<IBankAccountRepository, BankAccountRepository>();
            services.AddTransient<IErrorRepository, ErrorRepository>();

            return services;
        }
    }
}
