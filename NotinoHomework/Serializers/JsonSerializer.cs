using NotinoHomework.Api.Serializers.Abstractions;

namespace NotinoHomework.Api.Serializers
{
    internal class JsonSerializer : IJsonSerializer
    {
        private readonly System.Text.Json.JsonSerializerOptions options;

        public JsonSerializer() : this(GetDefaultJsonSerializerOptions())
        {
        }

        public JsonSerializer(System.Text.Json.JsonSerializerOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        async Task<Stream> IStreamSerializer.SerializeAsync<T>(T value, CancellationToken token)
        {
            using var ms = new MemoryStream();

            await System.Text.Json.JsonSerializer.SerializeAsync<T>(ms, value, options, token);

            return ms;
        }

        async Task<T> IStreamSerializer.DeserializeAsync<T>(Stream stream, CancellationToken token)
        {
            var deserializationResult = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(stream, options, token);

            if (deserializationResult is null) return new T();

            return deserializationResult;
        }

        T IByteSerializer.Deserialize<T>(byte[] value)
        {
            var result = System.Text.Json.JsonSerializer.Deserialize<T>(value, options);

            if (result is null) return new T();

            return result;
        }

        T IStringSerializer.Deserialize<T>(string value)
        {
            var result = System.Text.Json.JsonSerializer.Deserialize<T>(value, options);

            if (result is null) return new T();

            return result;
        }

        async Task<T> IByteSerializer.DeserializeAsync<T>(byte[] value, CancellationToken token)
        {
            using var ms = new MemoryStream(value);

            var result = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(ms, options, token);

            if (result is null) return new T();

            return result;
        }

        Task<T> IStringSerializer.DeserializeAsync<T>(string value, CancellationToken token)
        {
            var deserializationResult = System.Text.Json.JsonSerializer.Deserialize<T>(value, options);

            if (deserializationResult is null) return Task.FromResult(new T());

            return Task.FromResult(deserializationResult);
        }

        byte[] IByteSerializer.Serialize<T>(T value)
        {
            return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes<T>(value, options);
        }

        string IStringSerializer.Serialize<T>(T value)
        {
            return System.Text.Json.JsonSerializer.Serialize<T>(value, options);
        }

        async Task<byte[]> IByteSerializer.SerializeAsync<T>(T value, CancellationToken token)
        {
            using var ms = new MemoryStream();

            await System.Text.Json.JsonSerializer.SerializeAsync<T>(ms, value, options, token);

            return ms.ToArray();
        }

        Task<string> IStringSerializer.SerializeAsync<T>(T value, CancellationToken token)
        {
            var serializationResult = System.Text.Json.JsonSerializer.Serialize<T>(value, options);

            string result = serializationResult ?? string.Empty;

            return Task.FromResult(result);
        }

        private static System.Text.Json.JsonSerializerOptions GetDefaultJsonSerializerOptions()
        {
            return new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web);
        }
    }
}
