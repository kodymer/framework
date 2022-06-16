using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Vesta.Caching
{
    public static class DistributedCacheExtensions
    {
        public static async Task<T> GetOrAddAsync<T>(this IDistributedCache cache, string key, T value, CancellationToken cancellationToken = default)
        {
            var storedValue = await cache.GetAsync(key, cancellationToken);
            if (storedValue is not null)
            {
                value = await GetSpecificTypeAsync<T>(cache, key, cancellationToken);
            }
            else
            {
                await SetSpecificTypeAsync(cache, key, value, cancellationToken);
            }

            return value;
        }

        public static async Task<T> GetOrAddAsync<T>(this IDistributedCache cache, string key, Func<T> factory, CancellationToken cancellationToken = default)
        {
            object value = null;

            var storedValue = await cache.GetAsync(key, cancellationToken);
            if (storedValue is not null)
            {
                value = await GetSpecificTypeAsync<T>(cache, key, cancellationToken);
            }
            else
            {
                value = factory();

                await SetSpecificTypeAsync(cache, key, value, cancellationToken);
            }

            return (T)value;
        }

        public static async Task<T> GetOrAddAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> factory, CancellationToken cancellationToken = default)
        {
            object value = null;

            var storedValue = await cache.GetAsync(key, cancellationToken);
            if (storedValue is not null)
            {
                value = await GetSpecificTypeAsync<T>(cache, key, cancellationToken);
            }
            else
            {
                value = await factory();

                await SetSpecificTypeAsync(cache, key, value, cancellationToken);
            }

            return (T)value;
        }

        public static async Task<T> GetSpecificTypeAsync<T>(this IDistributedCache cache, string key, CancellationToken cancellationToken = default)
        {
            object value = null;

            var storedValue = await cache.GetAsync(key, cancellationToken);
            if (storedValue is not null)
            {
                // TODO: move to Vesta.Json package
                using (var memoryStream = new MemoryStream(storedValue))
                {
                    value = await JsonSerializer.DeserializeAsync(memoryStream, typeof(T), cancellationToken: cancellationToken);
                }
            }

            return (T)value;
        }

        public static async Task<T> SetSpecificTypeAsync<T>(this IDistributedCache cache, string key, T value, CancellationToken cancellationToken = default)
        {
            byte[] storedValue = null;

            // TODO: move to Vesta.Json package
            using (var memoryStream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(memoryStream, value, typeof(T), cancellationToken: cancellationToken);

                storedValue = memoryStream.ToArray();
            }
 
            await cache.SetAsync(key, storedValue, cancellationToken);

            return value;
        }
    }
}
