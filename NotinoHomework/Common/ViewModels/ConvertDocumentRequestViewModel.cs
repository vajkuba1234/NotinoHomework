using System.ComponentModel.DataAnnotations;

namespace NotinoHomework.Api.Common.ViewModels
{
    public class ConvertDocumentRequestViewModel
    {
        public FileType FileType { get; set; }

        [Required]
        public IFormFile FormFile { get; set; }


        public EmailViewModel? Email { get; set; }
    }
}
