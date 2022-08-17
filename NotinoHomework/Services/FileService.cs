using Microsoft.Extensions.Options;
using NotinoHomework.Api.Common;
using NotinoHomework.Api.Configs;
using NotinoHomework.Api.Serializers.Abstractions;
using NotinoHomework.Api.Services.Abstractions;
using NotinoHomework.Common;
using System.Security.AccessControl;

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
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileOptions.UploadPath);

            PrepareConvertedFilesDirectory(filePath);

            Stream? fileStream = await ConvertInternalAsync(convertFrom, convertTo, stream, token);

            if (fileStream is null) return string.Empty;

            await ((IFileService)this).SaveAsync(filePath, fileStream, token);

            return filePath;
        }

        async Task<Stream> IFileService.LoadAsync(string filePath)
        {
            var stream = new MemoryStream();

            using var reader = new System.IO.StreamReader(stream);

            string result = await reader.ReadToEndAsync();

            return stream;
        }

        async Task IFileService.SaveAsync(string filePath, Stream stream, CancellationToken token)
        {
            using var fileStream = new FileStream(filePath, FileMode.Create);

            await stream.CopyToAsync(fileStream, token);
        }

        // To get rid of switch or conditions this could be implemented as Strategy pattern
        private async Task<Stream> ConvertInternalAsync(Common.FileType from, Common.FileType to, Stream stream, CancellationToken token)
        {
            return (from, to) switch
            {
                (Common.FileType.XML, Common.FileType.JSON) => await ConvertXmlToJson(stream, token),
                (Common.FileType.JSON, Common.FileType.XML) => await ConvertJsonToXml(stream, token),
                _ => throw new NotImplementedException()
            };
        }

        private async Task<Stream> ConvertXmlToJson(Stream stream, CancellationToken token)
        {
            var document = await xmlSerializer.DeserializeAsync<Document>(stream, token);

            if (document is null) return new MemoryStream();

            var result = await ((IStreamSerializer)jsonSerializer).SerializeAsync(document, token);

            return result;
        }

        private async Task<Stream> ConvertJsonToXml(Stream stream, CancellationToken token)
        {
            var document = await jsonSerializer.DeserializeAsync<Document>(stream, token);

            if (document is null) return new MemoryStream();

            var result = await ((IStreamSerializer)xmlSerializer).SerializeAsync(document, token);

            return result;
        }

        private void PrepareConvertedFilesDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                var uploadDirectory = Directory.CreateDirectory(path);

                var accessControl = uploadDirectory.GetAccessControl();

                var fsAccessRule = new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow);
                accessControl.AddAccessRule(fsAccessRule);
                uploadDirectory.SetAccessControl(accessControl);
            }
        }
    }
}
