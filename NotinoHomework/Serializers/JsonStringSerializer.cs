using NotinoHomework.Api.Serializers.Abstractions;
using System.Text.Json;

namespace NotinoHomework.Api.Serializers
{
    public class JsonStringSerializer : IStringSerializer
    {
        private readonly JsonSerializerOptions options;

        public JsonStringSerializer() : this(GetDefaultJsonSerializerOptions())
        {
        }

        public JsonStringSerializer(JsonSerializerOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public T? Deserialize<T>(string value)
        {
            return JsonSerializer.Deserialize<T>(value, options);
        }

        public string Serialize<T>(T value)
        {
            return JsonSerializer.Serialize<T>(value, options);
        }

        private static JsonSerializerOptions GetDefaultJsonSerializerOptions()
        {
            return new JsonSerializerOptions(JsonSerializerDefaults.Web);
        }
    }
}
