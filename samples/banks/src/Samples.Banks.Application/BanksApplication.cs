using AutoMapper;
using AutoMapper.EquivalencyExpression;
using CompanyName.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Samples.Banks
{
    public static class BanksApplication
    {

        public static IServiceCollection AddBanksApplication(this IServiceCollection services, IConfiguration configuration)
        {

            services
                .AddBanksDomain(configuration)
                .AddBanksAppServices();

            services
                .AddCompanyNameAutoMapper()
                .AddCompanyNameHybridCaching()
                .AddSerializerFactory<HybridCacheSerializerFactory>()
                .UseRedis();

            services
                .Configure<JsonSerializerOptions>(options =>
                {
                    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.WriteIndented = false;
                    options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString;
                    options.Converters.Add(new CustomDecimalConverter());   
                });

            return services;
        }
    }
}
