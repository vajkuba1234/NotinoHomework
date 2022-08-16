﻿using NotinoHomework.Api.Common.ViewModels;
using NotinoHomework.Api.Configs;
using System.Net.Mail;

namespace NotinoHomework.Api.Services
{
    public interface IEmailService
    {
        Task SendAsync(string from, string to, string subject, Stream content, string fileName);
    }

    public class EmailService : IEmailService
    {
        private readonly IEmailOptions options;

        public EmailService(IEmailOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        async Task IEmailService.SendAsync(string from, string to, string subject, Stream content, string fileName)
        {
            string messageBody = "Check converted file in attachment.";

            var message = new MailMessage(from, to, subject, messageBody);
            var attachment = new Attachment(content, fileName);

            message.Attachments.Add(attachment);

            using var client = new SmtpClient();

            client.Host = options.Host;
            client.Port = options.Port;
            client.Credentials = new System.Net.NetworkCredential { UserName = options.Username, Password = options.Password };

            await client.SendMailAsync(message);
        }
    }
}
