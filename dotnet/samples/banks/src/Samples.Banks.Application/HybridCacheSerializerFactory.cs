using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Samples.Banks
{
    public class HybridCacheSerializerFactory : IHybridCacheSerializerFactory
    {
        private readonly IOptions<JsonSerializerOptions> _jsonOptions;

        public HybridCacheSerializerFactory(IOptions<JsonSerializerOptions> jsonOptions)
        {
            _jsonOptions = jsonOptions;
        }

        public bool TryCreateSerializer<T>([NotNullWhen(true)] out IHybridCacheSerializer<T> serializer)
        {
            serializer = new HybridCacheSerializer<T>(_jsonOptions);

            return true;
        }
    }


}
