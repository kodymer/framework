using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaCachingStackExchangeRedis(this IServiceCollection services, Action<RedisCacheOptions> configureOptions)
        {
            services.AddVestaCaching();

            services.AddStackExchangeRedisCache(configureOptions);
        }
    }
}
