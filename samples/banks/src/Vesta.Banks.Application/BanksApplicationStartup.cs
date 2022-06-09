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
        public static IServiceCollection AddBanksApplication(this IServiceCollection services, IConfiguration configuration)
        {

            services
                .AddBanksDomain(configuration)
                .AddBanksEntityFrameworkCore(configuration)
                .AddBanksDapper(configuration)
                .AddBanksAppSevices()
                .AddBanksEventHandlers()
                .AddBanksAutoMapper();

            services
                .AddVestaDddApplication();

            return services;
        }
    }
}
