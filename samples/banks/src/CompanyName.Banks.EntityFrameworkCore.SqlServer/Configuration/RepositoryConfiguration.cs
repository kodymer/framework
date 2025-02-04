using Microsoft.Extensions.DependencyInjection;
using CompanyName.Banks.EntityFrameworkCore.Repositories;
using CompanyName.Banks.Traceability;

namespace CompanyName.Banks.Configuration
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
