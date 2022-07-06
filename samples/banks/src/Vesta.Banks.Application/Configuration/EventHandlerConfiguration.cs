using Microsoft.Extensions.DependencyInjection;

namespace Vesta.Banks.Configuration
{
    public static class EventHandlerConfiguration
    {
        public static IServiceCollection AddBanksEventHandlers(this IServiceCollection services)
        {
            services.AddVestaEventHandlers(options =>
            {
                options.Add<BankAccountChangedEventHandler>();
                options.Add<BankAccountBalanceDecreasedEventHandler>();
                options.Add<BankAccountBalanceIncreasedEventHandler>();
            });

            return services;

        }
    }
}
