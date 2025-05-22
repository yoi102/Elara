using ChatService.WebAPI.Services;
using Initializer;
using Profile;
using Scalar.AspNetCore;
using UploadedItem;

var builder = WebApplication.CreateBuilder(args);

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
    options.Address = new Uri("https://localhost:8080/Elara/FileService");
});

builder.Services.AddGrpcClient<ProfileService.ProfileServiceClient>("ProfileServiceClient", options =>
{
    options.Address = new Uri("https://localhost:8080/Elara/PersonalSpaceService");
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
