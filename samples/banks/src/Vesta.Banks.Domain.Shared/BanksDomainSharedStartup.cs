using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vesta.Banks
{
    public static class BanksDomainSharedStartup
    {

        public static void AddBanksDomainShared(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddVestaDddDomainEventBus();
        }
    }
}