using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vesta.Banks.Bank;
using Vesta.Banks.Configuration;

namespace Vesta.Banks
{
    public static class BanksDomainStartup
    {
        public static void AddBanksDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IBankTransferService, BankTransferService>();
            
            services.AddDomainSevices();
        }
    }
}
