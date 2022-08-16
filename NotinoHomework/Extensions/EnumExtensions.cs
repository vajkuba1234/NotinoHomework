using NotinoHomework.Api.Common;

namespace NotinoHomework.Api.Extensions
{
    public static class EnumExtensions
    {
        public static string GetFileTypeExtension(this FileType fileType)
        {
            return fileType switch
            {
                FileType.JSON => "json",
                FileType.XML => "xml",
                _ => throw new NotImplementedException($"Uknown file type: {fileType}")
            };
        }
    }
}
