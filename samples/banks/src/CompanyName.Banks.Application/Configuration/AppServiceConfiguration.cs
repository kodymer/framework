using Microsoft.Extensions.DependencyInjection;
using CompanyName.Banks.Bank;

namespace CompanyName.Banks.Configuration
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
