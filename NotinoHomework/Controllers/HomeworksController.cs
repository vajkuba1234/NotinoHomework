using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotinoHomework.Api.Commands;
using NotinoHomework.Api.Common.ViewModels;

namespace NotinoHomework.Api.Controllers
{
    [Route("api/v1/homeworks")]
    [ApiController]
    public class HomeworksController : ControllerBase
    {
        private readonly ISender mediator;

        public HomeworksController(ISender mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("convert")]
        public async Task<IActionResult> ConvertFileAsync([FromForm] ConvertDocumentRequestViewModel viewModel)
        {
            var result = await mediator.Send(new ConvertDocumentCommand { ConvertTo = viewModel.FileType, FormFile = viewModel.FormFile });

            if (viewModel.Email is not null)
            {
                _ = await mediator.Send(new SendEmailCommand { From = viewModel.Email.From, To = viewModel.Email.To, Subject = viewModel.Email.Subject });
            }

            return File(result.Content, result.ContentType, result.FileName);
        }
    }
}
