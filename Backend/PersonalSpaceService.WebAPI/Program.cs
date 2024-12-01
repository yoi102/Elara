using Initializer;

var builder = WebApplication.CreateBuilder(args);


builder.ConfigureCommonServices(new InitializerOptions
{
    EventBusQueueName = "PersonalSpaceService.WebAPI",
    LogFileRelativePath = "PersonalSpaceService//log_.txt"
});
// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
