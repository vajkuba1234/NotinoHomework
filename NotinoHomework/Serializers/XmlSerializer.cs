namespace NotinoHomework.Serializers
{
    internal class XmlSerializer : ISerializer
    {
        T ISerializer.Deserialize<T>(string value)
        {
            throw new NotImplementedException();
        }

        string ISerializer.Serialize<T>(T value)
        {
            throw new NotImplementedException();
        }
    }
}
