using Newtonsoft.Json;

namespace NotinoHomework.Serializers
{
    internal class JsonSerializer : ISerializer
    {
        T ISerializer.Deserialize<T>(string value)
        {
            var deserializationResult = JsonConvert.DeserializeObject<T>(value);

            return deserializationResult;
        }

        string ISerializer.Serialize<T>(T value)
        {
            string serializationResult = JsonConvert.SerializeObject(value);

            return serializationResult;
        }
    }
}
