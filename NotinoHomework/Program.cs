using MediatR;
using NotinoHomework.Api.Common;
using NotinoHomework.Api.Configs;
using NotinoHomework.Api.Serializers;
using NotinoHomework.Api.Serializers.Abstractions;
using NotinoHomework.Api.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(EmailOptions.Email));

builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IFileService, FileService>();

builder.Services.AddTransient<IJsonSerializer, JsonSerializer>();
builder.Services.AddTransient<IXmlSerializer, XmlSerializer>();


var app = builder.Build();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseEndpoints(endpoints => endpoints.MapControllers());
app.MapControllers();

//app.MapGet("/homeworks/prefilled", async (ISender mediator, CancellationToken cancellationToken) =>
//{
//    var result = await mediator.Send(new GetDocumentQuery(), cancellationToken);

//    return Results.Ok(result);
//});

//app.MapPost("/homeworks/convert/minimal", async (ISender mediator, ConvertDocumentRequestViewModel viewModel) =>
//{
//    var result = await mediator.Send(new ConvertDocumentCommand { ConvertTo = viewModel.FileType, FormFile = viewModel.FormFile });

//    if (viewModel.Email is not null)
//    {
//        //_ = await mediator.Send(new SendEmailCommand { ... });
//    }

//    return Results.File(result.Content, result.ContentType, result.FileName);
//}).Accepts<IFormFile>("multipart/form-data").Accepts<ConvertDocumentRequestViewModel>("application/json").Produces(200);

app.Run();
