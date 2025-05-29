using FileService.Infrastructure.Services;
using FileService.WebAPI.Services;
using Initializer;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.ConfigureCommonServices(new InitializerOptions
{
    EventBusQueueName = "FileService.WebAPI",
    LogFileRelativePath = "FileService/log_.txt"
});
string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
string settingsFullPath = Path.Combine(programFilesPath, "Elara\\fileservice.appsettings.json");
builder.Configuration.AddJsonFile(settingsFullPath);

builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddGrpc();

builder.Services.Configure<SMBStorageOptions>(builder.Configuration.GetSection("FileService:SMB"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.MapGrpcService<UploadedItemServiceImplementation>();

app.UseStaticFiles();

app.UseCommonMiddleware();

app.MapControllers();

app.Run();
