using Initializer;

var builder = WebApplication.CreateBuilder(args);


builder.ConfigureCommonServices(new InitializerOptions
{
    EventBusQueueName = "PersonalSpaceService.WebAPI",
    LogFileRelativePath = "PersonalSpaceService//log_.txt"
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "PersonalSpaceService.WebAPI", Version = "v1" });
    c.EnableAnnotations();
});

builder.Services.AddDataProtection();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PersonalSpaceService.WebAPI v1"));
}

app.UseCommonMiddleware();

app.MapControllers();

app.Run();
