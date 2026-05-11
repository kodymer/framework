using System.Text;
using System.Text.Json;

namespace System
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Converts a UTF-8 encoded byte array to an object of type T using JSON deserialization.
        /// </summary>
        /// <typeparam name="T">The target type to deserialize to.</typeparam>
        /// <param name="bytes">The byte array to deserialize.</param>
        /// <param name="options">Optional JsonSerializerOptions.</param>
        /// <returns>Deserialized object of type T.</returns>
        public static T ToObject<T>(this byte[] bytes, JsonSerializerOptions options = null)
        {
            var json = Encoding.UTF8.GetString(bytes);
            return JsonSerializer.Deserialize<T>(json, options);
        }


        public static bool HasContent(this byte[] data)
        {
            return data is { Length: > 0 };
        }

    }
}
