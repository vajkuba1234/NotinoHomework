namespace NotinoHomework.Api.Common.ViewModels
{
    public class ConvertDocumentRequestViewModel
    {
        public EmailViewModel Email { get; set; }
        public FileType FileType { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
