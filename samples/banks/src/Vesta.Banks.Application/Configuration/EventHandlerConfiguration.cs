using Microsoft.Extensions.DependencyInjection;

namespace Vesta.Banks.Configuration
{
    public static class EventHandlerConfiguration
    {
        public static IServiceCollection AddBanksEventHandlers(this IServiceCollection services)
        {
            services.AddVestaEventHandlers(options =>
            {
                options.Add<CreateBankAccountEventHandler>();
            });

            return services;

        }
    }
}
