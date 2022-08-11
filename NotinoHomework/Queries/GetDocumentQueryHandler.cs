using MediatR;
using NotinoHomework.Common;

namespace NotinoHomework.Api.Queries
{
    public class GetDocumentQueryHandler : MediatR.IRequestHandler<GetDocumentQuery, Document>
    {
        Task<Document> IRequestHandler<GetDocumentQuery, Document>.Handle(GetDocumentQuery request, CancellationToken cancellationToken)
        {
            var response = new Document
            {
                Title = request.Title,
                Text = request.Text
            };

            return Task.FromResult(response);
        }
    }
}
