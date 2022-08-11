using NotinoHomework.Api.Serializers.Abstractions;
using System.Text.Json;

namespace NotinoHomework.Api.Serializers
{
    public class JsonStreamSerializer : IStreamSerializer
    {
        private readonly JsonSerializerOptions options;

        public JsonStreamSerializer() : this(GetDefaultJsonSerializerOptions())
        {
        }

        public JsonStreamSerializer(JsonSerializerOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public T? Deserialize<T>(Stream stream)
        {
            return JsonSerializer.Deserialize<T>(stream, options);
        }

        public ValueTask<T?> DeserializeAsync<T>(Stream stream)
        {
            return JsonSerializer.DeserializeAsync<T>(stream, options);
        }

        public Stream Serialize<T>(T value)
        {
            using var ms = new MemoryStream();

            JsonSerializer.Serialize<T>(ms, value, options);

            return ms;
        }

        public async Task<Stream> SerializeAsync<T>(T value)
        {
            using var ms = new MemoryStream();

            await JsonSerializer.SerializeAsync<T>(ms, value, options);

            return ms;
        }

        private static JsonSerializerOptions GetDefaultJsonSerializerOptions()
        {
            return new JsonSerializerOptions(JsonSerializerDefaults.Web);
        }
    }
}
