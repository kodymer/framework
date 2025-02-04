using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.Banks
{
    public static class BanksDomainSharedStartup
    {

        public static IServiceCollection AddBanksDomainShared(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCompanyNameDddDomainEventBus();

            return services;
        }
    }
}