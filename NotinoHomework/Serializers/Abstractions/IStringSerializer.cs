namespace NotinoHomework.Api.Serializers.Abstractions
{
    public interface IStringSerializer
    {
        string Serialize<T>(T value);
        Task<string> SerializeAsync<T>(T value, CancellationToken token = default);
        T Deserialize<T>(string value) where T : class, new();
        Task<T> DeserializeAsync<T>(string value, CancellationToken token = default) where T : class, new();
    }
}
