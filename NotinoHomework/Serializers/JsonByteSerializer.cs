using NotinoHomework.Api.Serializers.Abstractions;
using System.Text.Json;

namespace NotinoHomework.Api.Serializers
{
    public class JsonByteSerializer : IByteSerializer
    {
        private readonly JsonSerializerOptions options;

        public JsonByteSerializer() : this(GetDefaultJsonSerializerOptions())
        {
        }

        public JsonByteSerializer(JsonSerializerOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public T? Deserialize<T>(byte[] value)
        {
            return JsonSerializer.Deserialize<T>(value, options);
        }

        public byte[] Serialize<T>(T value)
        {
            return JsonSerializer.SerializeToUtf8Bytes<T>(value, options);
        }

        public async Task<byte[]> SerializeAsync<T>(T value, CancellationToken token = default)
        {
            using var ms = new MemoryStream();

            await JsonSerializer.SerializeAsync<T>(ms, value, options, token);

            return ms.GetBuffer();
        }

        public Task<T?> DeserializeAsync<T>(byte[] value, CancellationToken token = default)
        {
            using var ms = new MemoryStream(value);

            var result = JsonSerializer.DeserializeAsync<T>(ms, options, token);

            return result.AsTask();
        }

        private static JsonSerializerOptions GetDefaultJsonSerializerOptions()
        {
            return new JsonSerializerOptions(JsonSerializerDefaults.Web);
        }
    }
}
