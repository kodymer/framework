using Microsoft.Extensions.DependencyInjection;

namespace Samples.Banks
{
    internal static class RepositoryBanksDapper
    {

        internal static IServiceCollection AddBanksRepositories(this IServiceCollection services)
        {
            // Add your dapper repository here

            return services;
        }
    }
}
