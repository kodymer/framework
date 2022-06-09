using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vesta.Banks
{
    public static class BanksDomainSharedStartup
    {

        public static IServiceCollection AddBanksDomainShared(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddVestaDddDomainEventBus();

            return services;
        }
    }
}