using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using System.Buffers;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Samples.Banks
{
    public class HybridCacheSerializer<T> : IHybridCacheSerializer<T>
    {
        private readonly JsonSerializerOptions _jsonOptions;

        public HybridCacheSerializer(
            IOptions<JsonSerializerOptions> jsonOptions)
        {
            _jsonOptions = jsonOptions.Value ?? throw new ArgumentNullException(nameof(jsonOptions));
        }

        public T Deserialize(ReadOnlySequence<byte> source)
        {
            object value = null;

            var reader = new Utf8JsonReader(source);
            value = JsonSerializer.Deserialize<T>(ref reader, _jsonOptions);
            if (value is null)
            {
                throw new JsonException($"Failed to deserialize type {typeof(T).Name}");
            }

            return (T)value;

        }

        public void Serialize(T value, IBufferWriter<byte> target)
        {
            using (var writer = new Utf8JsonWriter(target))
            {
                JsonSerializer.Serialize(writer, value, _jsonOptions);

                writer.Flush();
            }
        }
    }

    public class CustomDecimalConverter : JsonConverter<decimal>
    {
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string stringValue = reader.GetString();
                if (int.TryParse(stringValue, out int value))
                {
                    return value;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetDecimal();
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
