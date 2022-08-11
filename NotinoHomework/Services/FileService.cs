namespace NotinoHomework.Api.Services
{
    public interface IFileService
    {
        Task<string> LoadAsync(Stream stream);
    }

    public class FileService : IFileService
    {
        async Task<string> IFileService.LoadAsync(Stream stream)
        {
            using var reader = new System.IO.StreamReader(stream);

            string result = await reader.ReadToEndAsync();

            return result;
        }
    }
}
