using Microsoft.Extensions.DependencyInjection;
using Vesta.Banks.Bank;
using Vesta.Banks.Domain.Bank;

namespace Vesta.Banks.Configuration
{
    public static class DomainServiceConfiguration
    {
        public static void AddBanksDomainSevices(this IServiceCollection services)
        {
            services.AddTransient<IBankAccountManager, BankAccountManager>();
            services.AddTransient<IBankTransferService, BankTransferService>();
        }
    }
}
