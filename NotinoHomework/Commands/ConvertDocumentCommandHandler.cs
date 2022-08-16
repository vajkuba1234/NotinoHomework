using MediatR;
using NotinoHomework.Api.Common;
using NotinoHomework.Api.Common.ViewModels;
using NotinoHomework.Api.Extensions;
using NotinoHomework.Api.Serializers.Abstractions;
using NotinoHomework.Common;

namespace NotinoHomework.Api.Commands
{
    public class ConvertDocumentCommandHandler : IRequestHandler<ConvertDocumentCommand, ConvertDocumentResponseViewModel>
    {
        private readonly IByteSerializer jsonSerializer;
        private readonly IByteSerializer xmlSerializer;

        public ConvertDocumentCommandHandler(Func<FileType, IByteSerializer?> serializerProvider)
        {
            ArgumentNullException.ThrowIfNull(nameof(serializerProvider));

            jsonSerializer = serializerProvider(FileType.JSON) ?? throw new ArgumentNullException(nameof(serializerProvider));
            xmlSerializer = serializerProvider(FileType.XML) ?? throw new ArgumentNullException(nameof(serializerProvider));
        }

        async Task<ConvertDocumentResponseViewModel> IRequestHandler<ConvertDocumentCommand, ConvertDocumentResponseViewModel>.Handle(ConvertDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = await DeserializeRequestData(request, cancellationToken);

            var outputStream = await SerializeResponseData(document, request, cancellationToken);

            string fileName = GetFileName(request);
            string contentType = GetContentType(request);

            var response = new ConvertDocumentResponseViewModel
            {
                Content = outputStream,
                FileName = fileName,
                ContentType = contentType
            };

            return response;
        }

        private async Task<Document> DeserializeRequestData(ConvertDocumentCommand request, CancellationToken token)
        {
            Document? document = null;

            var dataBuffer = await GetBuffer(request.FormFile);

            if (request.ConvertTo == Common.FileType.JSON)
            {
                document = await xmlSerializer.DeserializeAsync<Document>(dataBuffer, token);
            }
            else if (request.ConvertTo == Common.FileType.XML)
            {
                document = await jsonSerializer.DeserializeAsync<Document>(dataBuffer, token);
            }

            if (document is null)
            {
                return new Document();
            }

            return document;
        }

        private async Task<Stream> SerializeResponseData(Document document, ConvertDocumentCommand request, CancellationToken token)
        {
            byte[] streamData = null;

            if (request.ConvertTo == Common.FileType.JSON)
            {
                streamData = await jsonSerializer.SerializeAsync(document, token);
            }
            else if (request.ConvertTo == Common.FileType.XML)
            {
                streamData = await xmlSerializer.SerializeAsync(document, token);
            }

            if (streamData is null)
            {
                return new MemoryStream();
            }

            return new MemoryStream(streamData);
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

        private async Task<byte[]> GetBuffer(IFormFile formFile)
        {
            using var incommingFileStream = formFile.OpenReadStream();
            using var memoryStream = new MemoryStream();

            await incommingFileStream.CopyToAsync(memoryStream);

            return memoryStream.ToArray();
        }
    }
}
