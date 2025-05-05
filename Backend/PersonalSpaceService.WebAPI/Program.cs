using Identity;
using Initializer;
using Scalar.AspNetCore;

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
    options.Address = new Uri("https://localhost:8080/Elara/IdentityService");//Nginx
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
