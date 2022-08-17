namespace NotinoHomework.Api.Serializers.Abstractions
{
    public interface IByteSerializer
    {
        byte[] Serialize<T>(T value);
        Task<byte[]> SerializeAsync<T>(T value, CancellationToken token = default);
        T Deserialize<T>(byte[] value) where T : class, new();
        Task<T> DeserializeAsync<T>(byte[] value, CancellationToken token = default) where T : class, new();
    }
}
