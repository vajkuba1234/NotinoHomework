using NotinoHomework.Api.Serializers.Abstractions;
using System.Xml.Serialization;

namespace NotinoHomework.Api.Serializers
{
    public class XmlByteSerializer : IByteSerializer
    {
        public T? Deserialize<T>(byte[] value)
        {
            var serializer = new XmlSerializer(typeof(T));

            using var ms = new MemoryStream(value);

            return (T?)serializer.Deserialize(ms);
        }

        public byte[] Serialize<T>(T value)
        {
            var serializer = new XmlSerializer(typeof(T));

            using var ms = new MemoryStream();

            serializer.Serialize(ms, value);

            return ms.ToArray();
        }
    }
}
