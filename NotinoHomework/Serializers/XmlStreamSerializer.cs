using NotinoHomework.Api.Serializers.Abstractions;
using System.Xml;
using System.Xml.Serialization;

namespace NotinoHomework.Api.Serializers
{
    public class XmlStreamSerializer : IStreamSerializer
    {
        public T? Deserialize<T>(Stream stream)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            return (T?)serializer.Deserialize(stream);
        }

        public async ValueTask<T?> DeserializeAsync<T>(Stream stream)
        {
            using var reader = XmlReader.Create(stream, new XmlReaderSettings { Async = true });

            var xmlResult = await reader.ReadContentAsAsync(typeof(T), null);

            return (T)xmlResult;
        }

        public Stream Serialize<T>(T value)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using var ms = new MemoryStream();

            serializer.Serialize(ms, value);

            return ms;
        }

        public Task<Stream> SerializeAsync<T>(T value)
        {
            var serializer = new XmlSerializer(typeof(T));

            using var ms = new MemoryStream();
            using var writer = XmlWriter.Create(ms, new XmlWriterSettings { Async = true });

            serializer.Serialize(writer, value);

            throw new NotImplementedException();
        }
    }
}
