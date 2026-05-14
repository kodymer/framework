using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Samples.Banks
{
    public static class BanksDomainShared
    {
        public static IServiceCollection AddBanksDomainShared(this IServiceCollection services)
        {
            services.AddCompanyNameCore();

            return services;
        }
    }
}