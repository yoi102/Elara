using ChatService.WebAPI.Services;
using Initializer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Profile;
using Scalar.AspNetCore;
using UploadedItem;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(7042, listenOptions =>
//    {
//        listenOptions.UseHttps(); // 不带参数，使用 dev cert
//        listenOptions.Protocols = HttpProtocols.Http2;
//    });
//});


builder.ConfigureCommonServices(new InitializerOptions
{
    EventBusQueueName = "ChatService.WebAPI",
    LogFileRelativePath = "ChatService//log_.txt"
});

builder.Services.AddOpenApi();

builder.Services.AddDataProtection();
builder.Services.AddTransient<IMessageQueryService, MessageQueryService>();

builder.Services.AddGrpcClient<UploadedItemService.UploadedItemServiceClient>("UploadedItemServiceClient", options =>
{
    options.Address = new Uri("https://localhost:7176");
    //options.Address = new Uri("https://localhost:8080/Elara/FileService");
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
});

builder.Services.AddGrpcClient<ProfileService.ProfileServiceClient>("ProfileServiceClient", options =>
{
    options.Address = new Uri("https://localhost:7136");
    //options.Address = new Uri("https://localhost:8080/Elara/PersonalSpaceService");
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCommonMiddleware();

app.MapControllers();

app.Run();
