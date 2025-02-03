using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using CompanyName.Caching;
using CompanyName.Caching.StackExchangeRedis;
using static Microsoft.Extensions.Options.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        private const string RedisConfigurationConfig = "Redis:Configuration";
        private const string RedisInstanceNameConfig = "Redis:InstanceName";

        public static void AddCompanyNameCachingStackExchangeRedis(this IServiceCollection services, Action<RedisCacheOptions> configureOptions)
        {
            services.AddCompanyNameCaching();

            services.AddStackExchangeRedisCache(configureOptions);
        }

        public static void AddCompanyNameCachingStackExchangeRedis(this IServiceCollection services)
        {
            services.AddCompanyNameCaching();

            services.AddSingleton<IOptionsFactory<RedisCacheOptions>, RedisCacheOptionsFactory>();
            services.AddSingleton<IOptions<RedisCacheOptions>>(serviceProvider =>
             {
                 var options = serviceProvider.GetRequiredService<IOptionsFactory<RedisCacheOptions>>().Create(null);
                 return Create(options);
             });

            services.AddStackExchangeRedisCache(_ => { });
        }
    }
}