using Microsoft.Extensions.DependencyInjection;

namespace Vesta.Banks.Configuration
{
    public static class DomainServiceConfiguration
    {
        public static void AddBanksDomainSevices(this IServiceCollection services)
        {
            services.AddTransient<IBankAccountManager, BankAccountManager>();
            services.AddTransient<IBankTransferService, BankTransferService>();
            services.AddTransient<IBankAccountPublisher, BankAccountPublisher>();
        }
    }
}
