using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.Banks.Configuration
{
    public static class EventHandlerConfiguration
    {
        public static IServiceCollection AddBanksEventHandlers(this IServiceCollection services)
        {
            services.AddCompanyNameEventHandlers(options =>
            {
                options.Add<BankAccountCreatedEventHandler>();
                options.Add<BankAccountChangedEventHandler>();
                options.Add<BankAccountBalanceDecreasedEventHandler>();
                options.Add<BankAccountBalanceIncreasedEventHandler>();
            });

            return services;

        }
    }
}
