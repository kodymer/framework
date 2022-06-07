using System.Collections.Concurrent;

namespace System.Collections.Generic
{
    public static class VestaDictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.TryGetValue(key, out TValue value) ? value : default(TValue);
        }
    }
}