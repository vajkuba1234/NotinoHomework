namespace NotinoHomework.Api.Configs
{
    public interface IFileOptions
    {
        public string UploadPath { get; set; }
    }

    public class FileOptions : IFileOptions
    {
        public const string File = nameof(File);

        public string UploadPath { get; set; } = string.Empty;
    }
}
