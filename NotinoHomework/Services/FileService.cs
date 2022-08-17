using Microsoft.Extensions.Options;
using NotinoHomework.Api.Common;
using NotinoHomework.Api.Configs;
using NotinoHomework.Api.Serializers.Abstractions;
using NotinoHomework.Api.Services.Abstractions;
using NotinoHomework.Common;

namespace NotinoHomework.Api.Services
{
    public class FileService : IFileService
    {
        private readonly IFileOptions fileOptions;
        private readonly IJsonSerializer jsonSerializer;
        private readonly IXmlSerializer xmlSerializer;

        public FileService(IOptions<Configs.FileOptions> options, IJsonSerializer jsonSerializer, IXmlSerializer xmlSerializer)
        {
            ArgumentNullException.ThrowIfNull(nameof(options));

            this.fileOptions = options.Value;
            this.jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            this.xmlSerializer = xmlSerializer ?? throw new ArgumentNullException(nameof(xmlSerializer));
        }

        async Task<string> IFileService.ConvertAsync(Common.FileType convertFrom, FileType convertTo, Stream stream, string fileName, CancellationToken token)
        {
            string directoryPath = fileOptions.UploadPath;

            PrepareConvertedFilesDirectory(directoryPath);

            string filePath = Path.Combine(directoryPath, fileName);

            string fileContent = await ConvertInternalAsync(convertFrom, convertTo, stream, token);

            await ((IFileService)this).SaveAsync(filePath, fileContent, token);

            return filePath;
        }

        Stream IFileService.Load(string filePath)
        {
            return new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite, 4096, true);
        }

        async Task IFileService.SaveAsync(string filePath, string fileContent, CancellationToken token)
        {
            using System.IO.FileStream fs = System.IO.File.Create(filePath);
            var buffer = System.Text.Encoding.UTF8.GetBytes(fileContent);

            await fs.WriteAsync(buffer);
        }

        // To get rid of switch or conditions this could be implemented as Strategy pattern
        private async Task<string> ConvertInternalAsync(Common.FileType from, Common.FileType to, Stream stream, CancellationToken token)
        {
            return (from, to) switch
            {
                (Common.FileType.XML, Common.FileType.JSON) => await ConvertXmlToJson(stream, token),
                (Common.FileType.JSON, Common.FileType.XML) => await ConvertJsonToXml(stream, token),
                _ => throw new NotImplementedException()
            };
        }

        private async Task<string> ConvertXmlToJson(Stream stream, CancellationToken token)
        {
            var document = await xmlSerializer.DeserializeAsync<Document>(stream, token);

            if (document is null) return string.Empty;

            var result = await ((IStringSerializer)jsonSerializer).SerializeAsync(document, token);

            return result;
        }

        private async Task<string> ConvertJsonToXml(Stream stream, CancellationToken token)
        {
            var document = await jsonSerializer.DeserializeAsync<Document>(stream, token);

            if (document is null) return string.Empty;

            var result = await ((IStringSerializer)xmlSerializer).SerializeAsync(document, token);

            return result;
        }

        private void PrepareConvertedFilesDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }
    }
}
