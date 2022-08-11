namespace NotinoHomework.Api.Serializers.Abstractions
{
    public interface IStringSerializer
    {
        string Serialize<T>(T value);
        T? Deserialize<T>(string value);
    }
}
