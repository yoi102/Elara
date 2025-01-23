﻿using IdentityService.Domain.Entities;
using IdentityService.Infrastructure;
using IdentityService.WebAPI;
using Initializer;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureCommonServices(new InitializerOptions
{
    EventBusQueueName = "IdentifierService.WebAPI",
    LogFileRelativePath = "IdentifierService//log_.txt"
});

builder.Services.AddDataProtection();
builder.Services.AddOpenApi();

builder.Services.AddGrpc();
builder.Services.AddScoped<IPasswordHasher<User>, BCryptPasswordHasher<User>>();

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

var app = builder.Build();

app.MapGrpcService<IdentityService.WebAPI.Services.IdentifierService>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCommonMiddleware();

app.MapControllers();

app.Run();
