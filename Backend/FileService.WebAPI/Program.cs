﻿using FileService.Infrastructure.Services;
using FileService.WebAPI.Services;
using Initializer;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureCommonServices(new InitializerOptions
{
    EventBusQueueName = "FileService.WebAPI",
    LogFileRelativePath = "FileService/log_.txt"
});
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddGrpc();

builder.Services.Configure<SMBStorageOptions>(builder.Configuration.GetSection("FileService:SMB"));

var app = builder.Build();

app.MapGrpcService<UploadedItemService>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseStaticFiles();

app.UseCommonMiddleware();

app.MapControllers();

app.Run();
