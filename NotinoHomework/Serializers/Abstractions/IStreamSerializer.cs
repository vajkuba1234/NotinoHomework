namespace NotinoHomework.Api.Serializers.Abstractions
{
    public interface IStreamSerializer
    {
        Task<Stream> SerializeAsync<T>(T value, CancellationToken token = default);
        Task<T> DeserializeAsync<T>(Stream stream, CancellationToken token = default) where T : class, new();
    }
}
