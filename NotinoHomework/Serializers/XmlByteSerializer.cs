using NotinoHomework.Api.Serializers.Abstractions;
using System.Xml.Serialization;

namespace NotinoHomework.Api.Serializers
{
    public class XmlByteSerializer : IByteSerializer
    {
        private readonly XmlRootAttribute xmlRootAttribute;

        public XmlByteSerializer() : this(GetDefaultXmlRootAttribute())
        {
        }

        public XmlByteSerializer(XmlRootAttribute xmlRootAttribute)
        {
            this.xmlRootAttribute = xmlRootAttribute ?? throw new ArgumentNullException(nameof(xmlRootAttribute));
        }

        public T? Deserialize<T>(byte[] value)
        {
            var serializer = new XmlSerializer(typeof(T), xmlRootAttribute);

            using var ms = new MemoryStream(value);

            return (T?)serializer.Deserialize(ms);
        }

        public byte[] Serialize<T>(T value)
        {
            var serializer = new XmlSerializer(typeof(T), xmlRootAttribute);

            using var ms = new MemoryStream();

            serializer.Serialize(ms, value);

            return ms.ToArray();
        }

        private static XmlRootAttribute GetDefaultXmlRootAttribute()
        {
            return new XmlRootAttribute { ElementName = "root", IsNullable = true };
        }
    }
}
