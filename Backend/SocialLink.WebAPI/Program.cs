using Initializer;
using Microsoft.AspNetCore.Identity;
using SocialLink.Domain.Entities;
using SocialLink.infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.ReadAndSetHostBuilderConfiguration();

builder.ConfigureCommonServices(new InitializerOptions
{
    EventBusQueueName = "SocialLink.WebAPI",
    LogFileRelativePath = "SocialLink//log_.txt"
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SocialLink.WebAPI", Version = "v1" });
});

builder.Services.AddDataProtection();


IdentityBuilder idBuilder = builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
});
idBuilder.AddEntityFrameworkStores<SocialLinkDbContext>()
    .AddDefaultTokenProviders()
    .AddUserManager<IdUserManager>();





// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SocialLink.WebAPI v1"));
}

app.UseCommonMiddleware();


app.MapControllers();

app.Run();
