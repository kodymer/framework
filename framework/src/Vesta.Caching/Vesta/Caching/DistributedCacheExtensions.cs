using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Vesta.Caching
{
    public static class DistributedCacheExtensions
    {
        public static async Task<T> GetOrAddAsync<T>(this IDistributedCache cache, string key, T value, CancellationToken cancellationToken = default)
        {
            var storedValue = await cache.GetAsync(key, cancellationToken);
            if (!(storedValue is null))
            {
                // TODO: move to Vesta.Json package
                using (var memoryStream = new MemoryStream(storedValue))
                {
                    value = (T)await JsonSerializer.DeserializeAsync(memoryStream, typeof(T), cancellationToken: cancellationToken);
                }
            }
            else
            {
                // TODO: move to Vesta.Json package
                using (var memoryStream = new MemoryStream())
                {
                    await JsonSerializer.SerializeAsync(memoryStream, value, typeof(T), cancellationToken: cancellationToken);

                    await cache.SetAsync(key, memoryStream.ToArray(), cancellationToken);
                }
            }

            return value;
        }

        public static Task<T> GetOrAddAsync<T>(this IDistributedCache cache, string key, Func<T> value, CancellationToken cancellationToken = default)
        {
            return GetOrAddAsync<T>(cache, key, value.Invoke(), cancellationToken);
        }
    }
}
