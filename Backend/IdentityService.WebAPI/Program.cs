﻿using Identity;
using IdentityService.Domain.Entities;
using IdentityService.Infrastructure;
using Initializer;
using Microsoft.AspNetCore.Identity;
using Personal;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureCommonServices(new InitializerOptions
{
    EventBusQueueName = "IdentifierService.WebAPI",
    LogFileRelativePath = "IdentifierService//log_.txt"
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "IdentifierService.WebAPI", Version = "v1" });
});

builder.Services.AddDataProtection();

builder.Services.AddGrpc();

builder.Services.AddGrpcClient<Person.PersonClient>("PersonClient", options =>
{
    options.Address = new Uri("https://localhost:7120");
    //options.Address = new Uri("https://localhost:8080/Elara/PersonalSpaceService");//Nginx
});


IdentityBuilder idBuilder = builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
    options.User.RequireUniqueEmail = true;
});
idBuilder.AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders()
    .AddUserManager<IdUserManager>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapGrpcService<IdentityService.WebAPI.Services.IdentifierService>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentifierService.WebAPI v1"));
}

app.UseCommonMiddleware();

app.MapControllers();

app.Run();