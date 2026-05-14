using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Samples.Banks
{
    public static class BanksDomain
    {

        public static IServiceCollection AddBanksDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddBanksDomainShared()
                .AddBanksDomainServices();

            services
                .AddCompanyNameLocalization();

            return services;
        }
    }
}
