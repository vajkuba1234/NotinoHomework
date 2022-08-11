namespace NotinoHomework.Api.Serializers.Abstractions
{
    public interface IByteSerializer
    {
        byte[] Serialize<T>(T value);
        T? Deserialize<T>(byte[] value);
    }
}
