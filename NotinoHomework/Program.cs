using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotinoHomework.Api.Commands;
using NotinoHomework.Api.Common.ViewModels;
using NotinoHomework.Api.Queries;
using NotinoHomework.Api.Serializers;
using NotinoHomework.Api.Serializers.Abstractions;
using NotinoHomework.Api.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IByteSerializer, JsonByteSerializer>();
builder.Services.AddTransient<IByteSerializer, XmlByteSerializer>();
builder.Services.AddTransient<IStringSerializer, JsonStringSerializer>();
builder.Services.AddTransient<IStringSerializer, XmlStringSerializer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/homeworks/prefilled", async (ISender mediator, CancellationToken cancellationToken) =>
{
    var result = await mediator.Send(new GetDocumentQuery(), cancellationToken);

    return Results.Ok(result);
});

app.MapPost("/homeworks/convert", async (ISender mediator, [FromBody] ConvertDocumentRequestViewModel viewModel) =>
{
    var result = await mediator.Send(new ConvertDocumentCommand { ConvertTo = viewModel.FileType, FormFile = viewModel.FormFile });

    if (viewModel.Email is not null)
    {
        //_ = await mediator.Send(new SendEmailCommand { ... });
    }

    return Results.File(result.Content, "application/json", result.FileName);
});

app.Run();
