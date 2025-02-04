using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CompanyName.Banks.Configuration;

namespace CompanyName.Banks
{
    public static class BanksDomainStartup
    {

        public static IServiceCollection AddBanksDomain(this IServiceCollection services, IConfiguration configuration)
        {
            
            services
                .AddBanksDomainShared(configuration)
                .AddBanksDomainSevices();

            services
                .AddCompanyNameEventBusAzure();

            return services;
        }
    }
}
