using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace System
{
    public static class ObjectExtensions
    {
        public static T As<T>(this object obj)
            where T : class
        {
            return obj as T;
        }

        /// <summary>
        /// Converts any object to a UTF-8 encoded byte array using JSON serialization.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="options">Optional JsonSerializerOptions.</param>
        /// <returns>Byte array representing the serialized object.</returns>
        public static byte[] ToByteArray(this object obj, JsonSerializerOptions options = null)
        {
            var json = JsonSerializer.Serialize(obj, options);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}
