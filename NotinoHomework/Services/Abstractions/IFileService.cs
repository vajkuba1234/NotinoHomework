namespace NotinoHomework.Api.Services.Abstractions
{
    public interface IFileService
    {
        Task<string> ConvertAsync(Common.FileType convertFrom, Common.FileType convertTo, Stream stream, string fileName, CancellationToken token = default);
        Task<Stream> LoadAsync(string filePath);
        Task SaveAsync(string filePath, Stream stream, CancellationToken token = default);
    }
}
