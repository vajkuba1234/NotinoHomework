using MediatR;
using NotinoHomework.Api.Configs;
using NotinoHomework.Api.Middleware;
using NotinoHomework.Api.Serializers;
using NotinoHomework.Api.Serializers.Abstractions;
using NotinoHomework.Api.Services;
using NotinoHomework.Api.Services.Abstractions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(EmailOptions.Email));
builder.Services.Configure<NotinoHomework.Api.Configs.FileOptions>(builder.Configuration.GetSection(NotinoHomework.Api.Configs.FileOptions.File));

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddSingleton<IJsonSerializer, JsonSerializer>();
builder.Services.AddSingleton<IXmlSerializer, XmlSerializer>();


var app = builder.Build();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseEndpoints(endpoints => endpoints.MapControllers());
app.MapControllers();

//app.MapPost("api/v1/documents/convert/minimal", async (ISender mediator, ConvertDocumentRequestViewModel viewModel) =>
//{
//    var result = await mediator.Send(new ConvertDocumentCommand { ConvertTo = viewModel.FileType, FormFile = viewModel.FormFile });

//    if (viewModel.Email is not null)
//    {
//        //_ = await mediator.Send(new SendEmailCommand { ... });
//    }

//    return Results.File(result.Content, result.ContentType, result.FileName);
//}).Accepts<IFormFile>("multipart/form-data").Accepts<ConvertDocumentRequestViewModel>("application/json").Produces(200);

app.Run();
