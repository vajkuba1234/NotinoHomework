using System.ComponentModel.DataAnnotations;

namespace NotinoHomework.Api.Common.ViewModels
{
    public class ConvertDocumentRequestViewModel
    {
        public FileType ConvertFrom { get; set; }

        public FileType ConvertTo { get; set; }

        [Required]
        public IFormFile FormFile { get; set; }


        public EmailViewModel? Email { get; set; }
    }
}
