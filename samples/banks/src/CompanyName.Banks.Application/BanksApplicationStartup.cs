using AutoMapper.EquivalencyExpression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using CompanyName.Banks.Configuration;
using CompanyName.Banks.Dapper;
using CompanyName.Banks.EntityFrameworkCore;

namespace CompanyName.Banks
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
                .AddCompanyNameDddApplication();

            services
                .AddCompanyNameCachingStackExchangeRedis();

            services
                .AddCompanyNameAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
