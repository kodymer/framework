using Microsoft.Extensions.DependencyInjection;
using Samples.Banks.AccountManagement;
using Samples.Banks.MoneyTransfer;

namespace Samples.Banks
{
    internal static class ServiceBanksApplication
    {
        internal static IServiceCollection AddBanksAppServices(this IServiceCollection services)
        {
            services.AddTransient<IAccountManagementAppService, AccountManagementAppService>();
            services.AddTransient<IMoneyTransferAppService, MoneyTransferAppService>();

            return services;
        }
    }
}