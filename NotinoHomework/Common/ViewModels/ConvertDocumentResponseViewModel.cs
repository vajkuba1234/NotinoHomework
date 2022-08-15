namespace NotinoHomework.Api.Common.ViewModels
{
    public class ConvertDocumentResponseViewModel
    {
        public Stream Content { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
    }
}
