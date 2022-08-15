namespace NotinoHomework.Api.Commands
{
    public class SendEmailCommand : MediatR.IRequest
    {
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
    }
}
