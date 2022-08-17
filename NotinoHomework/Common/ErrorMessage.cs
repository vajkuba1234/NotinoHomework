using System.Text.Json;

namespace NotinoHomework.Api.Common
{
    public class ErrorMessage
    {
        public int StatusCode { get; set; } = default;
        public string Message { get; set; } = string.Empty;

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
