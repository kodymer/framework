using CompanyName.Caching;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCompanyNameCaching(this IServiceCollection services, Action<MemoryCacheOptions> memoryCacheOptions = null)
        {
            services
                .AddCompanyNameCore();

            if (memoryCacheOptions is not null)
            {
                services
                    .AddMemoryCache(memoryCacheOptions)
                    .AddDistributedMemoryCache()
                    .Configure<MemoryDistributedCacheOptions>(memoryCacheOptions.Invoke);
            }
            else
            {
                services
                    .AddMemoryCache()
                    .AddDistributedMemoryCache();
            }

            return services;
        }


        public static IHybridCacheBuilder AddCompanyNameHybridCaching(this IServiceCollection services, Action<HybridCacheOptions> configure = null)
        {
            services
                .AddCompanyNameCaching();

            var builder = configure is not null ?
                services.AddHybridCache(configure) :
                services.AddHybridCache();

            return builder;
        }

        public static IHybridCacheBuilder AddCompanyNameHybridCaching<T>(this IServiceCollection services, Action<HybridCacheOptions> configure = null, IHybridCacheSerializer<T> serializer = null, IHybridCacheSerializerFactory serializerFactory = null)
        {

            var builder = AddCompanyNameHybridCaching(services, configure);

            if (serializer is not null)
            {
                builder
                    .AddSerializer(serializer);
            }

            if (serializerFactory is not null)
            {
                builder
                    .AddSerializerFactory(serializerFactory);
            }

            return builder;
        }
    }
}
