using IdentityService.Domain.Entities;
using IdentityService.Infrastructure;
using Initializer;
using Microsoft.AspNetCore.Identity;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.ReadAndSetHostBuilderConfiguration();
var logFolder = builder.Configuration["LogFolder"];
ArgumentException.ThrowIfNullOrEmpty(logFolder);

var logFilePath = Path.Combine(logFolder, "IdentityService/log-.txt");
var eventBusQueueName = "IdentityService.WebAPI";
builder.ConfigureCommonServices(new InitializerOptions
{
    EventBusQueueName = eventBusQueueName,
    LogFilePath = logFilePath
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "IdentityService.WebAPI", Version = "v1" });
});

builder.Services.AddDataProtection();

IdentityBuilder identityBuilder = builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
}
);
identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(Role), builder.Services);
identityBuilder.AddEntityFrameworkStores<IdentityDbContext>().AddDefaultTokenProviders()
    .AddRoleManager<RoleManager<Role>>()
    .AddUserManager<IdentityUserManager>();

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityService.WebAPI v1"));
}

app.UseCommonMiddleware();

app.MapControllers();

app.Run();