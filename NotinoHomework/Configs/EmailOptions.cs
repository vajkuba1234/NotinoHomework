namespace NotinoHomework.Api.Configs
{
    public interface IEmailOptions
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class EmailOptions : IEmailOptions
    {
        public const string Email = nameof(Email);

        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}
