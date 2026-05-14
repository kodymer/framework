using Microsoft.Extensions.DependencyInjection;
using Samples.Banks.Accounts;
using Samples.Banks.Transfers;

namespace Samples.Banks
{
    internal static class ServiceBanksDomain
    {

        internal static void AddBanksDomainServices(this IServiceCollection services)
        {
            services
                .AddTransient<IBankAccountManager, BankAccountManager>()
                .AddTransient<IBankTransferService, BankTransferService>()
                .AddTransient<IBankAccountPublisher, BankAccountPublisher>();
        }
    }
}