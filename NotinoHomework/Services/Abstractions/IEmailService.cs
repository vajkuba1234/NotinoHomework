namespace NotinoHomework.Api.Services.Abstractions
{
    public interface IEmailService
    {
        Task SendAsync(string from, string to, string subject, Stream content, string fileName, CancellationToken token = default);
    }
}
