using MediatR;
using NotinoHomework.Api.Common.ViewModels;
using NotinoHomework.Api.Extensions;
using NotinoHomework.Api.Services.Abstractions;

namespace NotinoHomework.Api.Commands
{
    public class ConvertDocumentCommandHandler : IRequestHandler<ConvertDocumentCommand, ConvertDocumentResponseViewModel>
    {
        private readonly IFileService fileService;

        public ConvertDocumentCommandHandler(IFileService fileService)
        {
            this.fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        async Task<ConvertDocumentResponseViewModel> IRequestHandler<ConvertDocumentCommand, ConvertDocumentResponseViewModel>.Handle(ConvertDocumentCommand request, CancellationToken cancellationToken)
        {
            var formFileStream = request.FormFile.OpenReadStream();
            string fileName = GetFileName(request);

            string convertedFilePath = await fileService.ConvertAsync(request.ConvertFrom, request.ConvertTo, formFileStream, fileName, cancellationToken);

            var outputStream = await fileService.LoadAsync(convertedFilePath);

            string contentType = GetContentType(request);

            var response = new ConvertDocumentResponseViewModel
            {
                Content = outputStream,
                FileName = fileName,
                ContentType = contentType
            };

            return response;
        }

        private string GetFileName(ConvertDocumentCommand request)
        {
            string formattedDateTime = DateTime.Now.ToString("yyyy_MM_dd-THH_mm_ss");
            string fileExtension = request.ConvertTo.GetFileTypeExtension();

            return $"converted-document-{formattedDateTime}.{fileExtension}";
        }

        private string GetContentType(ConvertDocumentCommand request)
        {
            if (request.ConvertTo == Common.FileType.XML)
            {
                return "application/xml";
            }

            return "application/json";
        }
    }
}
