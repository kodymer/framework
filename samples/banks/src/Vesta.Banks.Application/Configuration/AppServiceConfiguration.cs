using Microsoft.Extensions.DependencyInjection;
using Vesta.Banks.Bank;

namespace Vesta.Banks.Configuration
{
    public static class AppServiceConfiguration
    {
        public static IServiceCollection AddBanksAppSevices(this IServiceCollection services)
        {
            services.AddTransient<IBankAppService, BankAppService>();

            return services;
        }
    }
}
