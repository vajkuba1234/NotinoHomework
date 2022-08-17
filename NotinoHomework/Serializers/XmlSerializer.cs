using NotinoHomework.Api.Serializers.Abstractions;

namespace NotinoHomework.Api.Serializers
{
    internal class XmlSerializer : IXmlSerializer
    {
        private readonly System.Xml.Serialization.XmlRootAttribute xmlRootAttribute;

        public XmlSerializer() : this(GetDefaultXmlRootAttribute())
        {
        }

        public XmlSerializer(System.Xml.Serialization.XmlRootAttribute xmlRootAttribute)
        {
            this.xmlRootAttribute = xmlRootAttribute ?? throw new ArgumentNullException(nameof(xmlRootAttribute));
        }

        Task<Stream> IStreamSerializer.SerializeAsync<T>(T value, CancellationToken token)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T), xmlRootAttribute);

            using var ms = new MemoryStream();

            serializer.Serialize(ms, value);

            return Task.FromResult((Stream)ms);
        }

        Task<T> IStreamSerializer.DeserializeAsync<T>(Stream stream, CancellationToken token)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T), xmlRootAttribute);

            var deserializationResult = serializer.Deserialize(stream);

            if (deserializationResult is null) return Task.FromResult(new T());

            return Task.FromResult((T)deserializationResult);
        }

        T IByteSerializer.Deserialize<T>(byte[] value)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T), xmlRootAttribute);

            using var ms = new MemoryStream(value);

            var result = serializer.Deserialize(ms);

            if (result is null) return new T();

            return (T)result;
        }

        T IStringSerializer.Deserialize<T>(string value)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using var reader = new StringReader(value);

            var result = serializer.Deserialize(reader);

            if (result is null) return new T();

            return (T)result;
        }

        Task<T> IByteSerializer.DeserializeAsync<T>(byte[] value, CancellationToken token)
        {
            var deserializationResult = ((IByteSerializer)this).Deserialize<T>(value);

            return Task.FromResult(deserializationResult);
        }

        Task<T> IStringSerializer.DeserializeAsync<T>(string value, CancellationToken token)
        {
            var deserializationResult = ((IStringSerializer)this).Deserialize<T>(value);

            return Task.FromResult(deserializationResult);
        }

        byte[] IByteSerializer.Serialize<T>(T value)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T), xmlRootAttribute);

            using var ms = new MemoryStream();

            serializer.Serialize(ms, value);

            return ms.ToArray();
        }

        string IStringSerializer.Serialize<T>(T value)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using var writer = new Common.Utf8StringWriter();

            serializer.Serialize(writer, value);

            return writer.ToString();
        }

        Task<byte[]> IByteSerializer.SerializeAsync<T>(T value, CancellationToken token)
        {
            var serializationResult = ((IByteSerializer)this).Serialize<T>(value);

            return Task.FromResult(serializationResult);
        }

        Task<string> IStringSerializer.SerializeAsync<T>(T value, CancellationToken token)
        {
            var serializationResult = ((IStringSerializer)this).Serialize<T>(value);

            string result = serializationResult ?? string.Empty;

            return Task.FromResult(result);
        }

        private static System.Xml.Serialization.XmlRootAttribute GetDefaultXmlRootAttribute()
        {
            return new System.Xml.Serialization.XmlRootAttribute { ElementName = "root", IsNullable = true };
        }
    }
}
