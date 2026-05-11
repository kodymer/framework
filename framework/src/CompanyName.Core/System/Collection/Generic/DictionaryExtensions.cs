using System.Collections.Concurrent;

namespace System.Collection.Generic
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.TryGetValue(key, out TValue value) ? value : default;
        }
    }
}