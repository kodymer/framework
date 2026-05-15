using CompanyName.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using static Microsoft.Extensions.Options.Options;

namespace CompanyName.Caching
{
    public static class HybridCacheBuilderExtensions
    {
        public static IHybridCacheBuilder UseRedis(this IHybridCacheBuilder provider, Action<RedisCacheOptions> configureOptions)
        {
            var services = provider.Services;

            services
                .AddStackExchangeRedisCache(configureOptions);

            return provider;
        }

        public static IHybridCacheBuilder UseRedis(this IHybridCacheBuilder provider)
        {
            var services = provider.Services;

            services
                .AddCompanyNameCaching()
                .AddSingleton<IOptionsFactory<RedisCacheOptions>, RedisCacheOptionsFactory>()
                .AddSingleton(serviceProvider =>
                 {
                     var options = serviceProvider.GetRequiredService<IOptionsFactory<RedisCacheOptions>>().Create(DefaultName);
                     return Create(options);
                 })
                .AddStackExchangeRedisCache(_ => { });

            return provider;
        }
    }
}