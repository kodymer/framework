using Microsoft.Extensions.DependencyInjection;
using Samples.Banks.EntityFrameworkCore;

namespace Samples.Banks
{
    internal static class RepositoryBanksEntityFrameworkCore
    {

        internal static IServiceCollection AddBanksRepositories(this IServiceCollection services)
        {
            services.AddRepositories<BankDbContext>();

            return services;
        }
    }
}
