using AutoMapper.EquivalencyExpression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vesta.Banks.Configuration;
using Vesta.Banks.Dapper;
using Vesta.Banks.EntityFrameworkCore;

namespace Vesta.Banks
{
    public static class BanksApplicationStartup
    {
        public static void AddBanksApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddVestaDddApplication();

            services.AddBanksDomain(configuration);
            services.AddBanksEntityFrameworkCore(configuration);
            services.AddBanksDapper(configuration);

            services.AddBanksAppSevices();
            services.AddBanksEventHandlers();
            services.AddBanksAutoMapper();
        }
    }
}
