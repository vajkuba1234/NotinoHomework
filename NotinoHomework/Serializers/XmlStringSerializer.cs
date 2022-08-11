using NotinoHomework.Api.Serializers.Abstractions;
using System.Text;
using System.Xml.Serialization;

namespace NotinoHomework.Api.Serializers
{
    public class XmlStringSerializer : IStringSerializer
    {
        public T? Deserialize<T>(string value)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using var reader = new StringReader(value);

            return (T?)serializer.Deserialize(reader);
        }

        public string Serialize<T>(T value)
        {
            var serializer = new XmlSerializer(typeof(T));

            using var writer = new Utf8StringWriter();

            serializer.Serialize(writer, value);

            return writer.ToString();
        }
    }

    class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}
