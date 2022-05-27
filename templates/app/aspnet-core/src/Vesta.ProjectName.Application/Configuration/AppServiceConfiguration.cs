using Microsoft.Extensions.DependencyInjection;
using Vesta.ProjectName.Bank;

namespace Vesta.ProjectName.Configuration
{
    public static class AppServiceConfiguration
    {
        public static void AddAppSevices(this IServiceCollection services)
        {
            services.AddTransient<IBankAppService, BankAppService>();
        }
    }
}
