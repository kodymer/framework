using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vesta.Banks.Configuration;
using Vesta.Banks.EntityFrameworkCore;

namespace Vesta.Banks
{
    public static class BanksApplicationStartup
    {
        public static void AddBanksApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBanksDomain(configuration);
            services.AddBanksEntityFrameworkCore(configuration);

            services.AddAppSevices();
        }
    }
}
