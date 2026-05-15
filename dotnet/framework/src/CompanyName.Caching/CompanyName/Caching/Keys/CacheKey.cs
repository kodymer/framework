using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CompanyName.Caching.Keys
{
    /// <summary>
    /// Provides helper methods to build deterministic cache keys from objects.
    /// The key is constructed from optional prefix segments and a truncated SHA-256
    /// hash of the JSON representation (camelCase).
    /// </summary>
    public static class CacheKey
    {
        /// <summary>
        /// Builds a cache key for the specified object using the default separator '-'.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize into the key.</typeparam>
        /// <param name="input">The object instance to include in the key; serialized to JSON using camelCase naming.</param>
        /// <param name="prefixes">Optional prefix segments that will be joined with the separator and placed before the hash. Empty or whitespace-only prefixes are ignored.</param>
        /// <returns>
        /// A cache key string composed of the joined prefixes (if any) followed by the separator and
        /// a 16-byte truncated SHA-256 hex digest of the JSON.
        /// </returns>
        public static string Build<T>(T input, params string[] prefixes)
        {
            return Build(input, '-', prefixes);
        }

        /// <summary>
        /// Builds a cache key for the specified DTO using the provided separator character.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize into the key.</typeparam>
        /// <param name="input">The object instance to include in the key; serialized to JSON using camelCase naming.</param>
        /// <param name="separator">The character used to join prefix segments and to separate prefixes from the hash.</param>
        /// <param name="prefixes">Optional prefix segments that will be joined with <paramref name="separator"/> and placed before the hash. Empty or whitespace-only prefixes are ignored.</param>
        /// <returns>
        /// A cache key string composed of the joined prefixes (if any) followed by <paramref name="separator"/>
        /// and a 16-byte truncated SHA-256 hex digest of the JSON.
        /// </returns>
        public static string Build<T>(T input, char separator, params string[] prefixes)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            var json = JsonSerializer.Serialize(input, options);

            using var sha = SHA256.Create();

            var digest = sha.ComputeHash(Encoding.UTF8.GetBytes(json));

            var hex = Convert.ToHexString(digest.AsSpan(0, 16));

            var prefix = string.Join(separator, prefixes.Where(p => !string.IsNullOrWhiteSpace(p.Trim())));

            return string.Format("{0}{1}{2}", prefix, !string.IsNullOrEmpty(prefix) ? separator : string.Empty, hex);
        }
    }
}