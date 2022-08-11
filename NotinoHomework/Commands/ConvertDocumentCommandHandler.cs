using MediatR;
using NotinoHomework.Api.Common.ViewModels;
using NotinoHomework.Api.Serializers;
using NotinoHomework.Api.Serializers.Abstractions;
using NotinoHomework.Common;

namespace NotinoHomework.Api.Commands
{
    public class ConvertDocumentCommandHandler : IRequestHandler<ConvertDocumentCommand, ConvertDocumentResponseViewModel>
    {
        private readonly IByteSerializer byteSerializer;

        public ConvertDocumentCommandHandler(IByteSerializer byteSerializer)
        {
            this.byteSerializer = byteSerializer ?? throw new ArgumentNullException(nameof(byteSerializer));
        }

        async Task<ConvertDocumentResponseViewModel> IRequestHandler<ConvertDocumentCommand, ConvertDocumentResponseViewModel>.Handle(ConvertDocumentCommand request, CancellationToken cancellationToken)
        {
            string fileName = request.FormFile.FileName;

            var document = await DeserializeRequestData(request);

            var outputStream = SerializeResponseData(document, request);

            var response = new ConvertDocumentResponseViewModel
            {
                Content = outputStream,
                FileName = $"converted-{fileName}"
            };

            return response;
        }

        private async Task<Document> DeserializeRequestData(ConvertDocumentCommand request)
        {
            Document? document = null;

            var dataBuffer = await GetBuffer(request.FormFile);

            if (request.ConvertTo == Common.FileType.JSON)   //xml => json
            {
                document = ((XmlByteSerializer)byteSerializer).Deserialize<Document>(dataBuffer);
            }
            else if (request.ConvertTo == Common.FileType.XML)   //json => xml
            {
                document = ((JsonByteSerializer)byteSerializer).Deserialize<Document>(dataBuffer);
            }

            if (document is null)
            {
                return new Document();
            }

            return document;
        }

        private Stream SerializeResponseData(Document document, ConvertDocumentCommand request)
        {
            byte[] streamData = null;

            if (request.ConvertTo == Common.FileType.JSON)
            {
                streamData = ((JsonByteSerializer)byteSerializer).Serialize(document);
            }
            else if (request.ConvertTo == Common.FileType.XML)
            {
                streamData = ((XmlByteSerializer)byteSerializer).Serialize(document);
            }

            if (streamData is null)
            {
                return new MemoryStream();
            }

            return new MemoryStream(streamData);
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
