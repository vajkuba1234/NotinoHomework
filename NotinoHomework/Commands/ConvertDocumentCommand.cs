using NotinoHomework.Api.Common;
using NotinoHomework.Api.Common.ViewModels;

namespace NotinoHomework.Api.Commands
{
    public class ConvertDocumentCommand : MediatR.IRequest<ConvertDocumentResponseViewModel>
    {
        public FileType ConvertTo { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
