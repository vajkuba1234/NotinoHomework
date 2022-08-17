using Microsoft.AspNetCore.Mvc;
using NotinoHomework.Api.Common.ViewModels;

namespace NotinoHomework.Contracts.Client
{
    public interface INotinoHomeworkClient
    {
        Task<IActionResult> ConvertFileAsync([FromForm] ConvertDocumentRequestViewModel viewModel, CancellationToken token);
    }
}
