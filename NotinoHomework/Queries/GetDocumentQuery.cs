using NotinoHomework.Common;

namespace NotinoHomework.Api.Queries
{
    public class GetDocumentQuery : MediatR.IRequest<Document>
    {
        public string Title { get; set; } = "Get title";
        public string Text { get; set; } = "Get text";
    }
}
