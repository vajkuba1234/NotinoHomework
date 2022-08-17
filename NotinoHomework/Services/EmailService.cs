using Microsoft.Extensions.Options;
using NotinoHomework.Api.Configs;
using NotinoHomework.Api.Services.Abstractions;
using System.Net.Mail;

namespace NotinoHomework.Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailOptions options;

        public EmailService(IOptions<EmailOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            this.options = options.Value;
        }

        async Task IEmailService.SendAsync(string from, string to, string subject, Stream content, string fileName, CancellationToken token)
        {
            string messageBody = "Check converted file in attachment.";

            var message = new MailMessage(from, to, subject, messageBody);
            var attachment = new Attachment(content, fileName);

            message.Attachments.Add(attachment);

            using var client = new SmtpClient();

            client.Host = options.Host;
            client.Port = options.Port;
            client.Credentials = new System.Net.NetworkCredential { UserName = options.Username, Password = options.Password };

            await client.SendMailAsync(message, token);
        }
    }
}
