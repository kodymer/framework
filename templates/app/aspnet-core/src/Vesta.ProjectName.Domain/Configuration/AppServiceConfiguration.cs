using Microsoft.Extensions.DependencyInjection;
using Vesta.ProjectName.Bank;
using Vesta.ProjectName.Domain.Bank;

namespace Vesta.ProjectName.Configuration
{
    public static class DomainServiceConfiguration
    {
        public static void AddDomainSevices(this IServiceCollection services)
        {
            services.AddTransient<IBankAccountManager, BankAccountManager>();
            services.AddTransient<IBankTransferService, BankTransferService>();
        }
    }
}
