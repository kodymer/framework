using AutoMapper.EquivalencyExpression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Vesta.Banks.Configuration;
using Vesta.Banks.Dapper;
using Vesta.Banks.EntityFrameworkCore;

namespace Vesta.Banks
{
    public static class BanksApplicationStartup
    {

        public const string RedisConfigurationConfig = "Redis:Configuration";

        public static IServiceCollection AddBanksApplication(this IServiceCollection services, IConfiguration configuration)
        {

            services
                .AddBanksDomain(configuration)
                .AddBanksEntityFrameworkCore(configuration)
                .AddBanksDapper(configuration)
                .AddBanksAppSevices()
                .AddBanksEventHandlers();

            services
                .AddVestaDddApplication();

            services
                .AddVestaCachingStackExchangeRedis(options =>
                {
                    options.Configuration = configuration.GetValue<string>(RedisConfigurationConfig);
                    options.InstanceName = "Vesta.Banks.";  //<--  Prefix key: Vesta.Banks.{Key}
                });

            services
                .AddVestaAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
