using Initializer;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureCommonServices(new InitializerOptions
{
    EventBusQueueName = "ChatService.WebAPI",
    LogFileRelativePath = "ChatService//log_.txt"
});

builder.Services.AddOpenApi();

builder.Services.AddDataProtection();


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
