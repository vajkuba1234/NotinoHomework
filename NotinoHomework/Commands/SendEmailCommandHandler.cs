using MediatR;

namespace NotinoHomework.Api.Commands
{
    public class SendEmailCommandHandler : MediatR.IRequestHandler<SendEmailCommand>
    {
        Task<Unit> IRequestHandler<SendEmailCommand, Unit>.Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {


            throw new NotImplementedException();
        }
    }
}
