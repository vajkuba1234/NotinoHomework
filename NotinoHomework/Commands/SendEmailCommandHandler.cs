using MediatR;
using NotinoHomework.Api.Services.Abstractions;

namespace NotinoHomework.Api.Commands
{
    public class SendEmailCommandHandler : MediatR.IRequestHandler<SendEmailCommand>
    {
        private readonly IEmailService emailService;

        public SendEmailCommandHandler(IEmailService emailService)
        {
            this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        async Task<Unit> IRequestHandler<SendEmailCommand, Unit>.Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            await emailService.SendAsync(request.From, request.To, request.Subject, request.Attachment, request.FileName, cancellationToken);

            return Unit.Value;
        }
    }
}
