using FileService.Infrastructure.Services;
using Initializer;

var builder = WebApplication.CreateBuilder(args);
builder.ReadAndSetHostBuilderConfiguration();
builder.ConfigureCommonServices(new InitializerOptions
{
    EventBusQueueName = "FileService.WebAPI",
    LogFileRelativePath = "FileService/log_.txt"
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "FileService.WebAPI", Version = "v1" });
});

builder.Services.Configure<SMBStorageOptions>(builder.Configuration.GetSection("FileService:SMB"));

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FileService.WebAPI v1"));
}

app.UseStaticFiles();

app.UseCommonMiddleware();

app.MapControllers();

app.Run();