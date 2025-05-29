using Identity;
using Initializer;
using Scalar.AspNetCore;
using UploadedItem;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureCommonServices(new InitializerOptions
{
    EventBusQueueName = "PersonalSpaceService.WebAPI",
    LogFileRelativePath = "PersonalSpaceService//log_.txt"
});

builder.Services.AddOpenApi();

builder.Services.AddDataProtection();

builder.Services.AddGrpc();

builder.Services.AddGrpcClient<Identifier.IdentifierClient>("IdentifierClient", options =>
{
    options.Address = new Uri("https://localhost:7135");
    //options.Address = new Uri("https://localhost:8080/Elara/grpc/IdentityService");
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
});

builder.Services.AddGrpcClient<UploadedItemService.UploadedItemServiceClient>("UploadedItemServiceClient", options =>
{
    options.Address = new Uri("https://localhost:7136");
    //options.Address = new Uri("https://localhost:8080/Elara/grpc/FileService");
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
