﻿namespace NotinoHomework.Api.Services.Abstractions
{
    public interface IFileService
    {
        Task<string> ConvertAsync(Common.FileType convertFrom, Common.FileType convertTo, Stream stream, string fileName, CancellationToken token = default);
        Stream Load(string filePath);
        Task SaveAsync(string filePath, string fileContent, CancellationToken token = default);
    }
}
