using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vesta.Banks.Configuration;

namespace Vesta.Banks
{
    public static class BanksDomainStartup
    {

        public static IServiceCollection AddBanksDomain(this IServiceCollection services, IConfiguration configuration)
        {
            
            services
                .AddBanksDomainShared(configuration)
                .AddBanksDomainSevices();

            services
                .AddVestaEventBusAzure();

            return services;
        }
    }
}
