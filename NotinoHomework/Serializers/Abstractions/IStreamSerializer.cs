namespace NotinoHomework.Api.Serializers.Abstractions
{
    public interface IStreamSerializer
    {
        Stream Serialize<T>(T value);
        Task<Stream> SerializeAsync<T>(T value);
        T? Deserialize<T>(Stream stream);
        ValueTask<T?> DeserializeAsync<T>(Stream stream);
    }
}
