using ASPNETCore;
using Commons.Extensions;
using Commons.Helpers;
using EventBus;
using EventBus.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.EFCore;
using JWT;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Initializer
{
    public static class WebApplicationBuilderExtensions
    {
        public static void ConfigureCommonServices(this WebApplicationBuilder builder, InitializerOptions initOptions)
        {
            ReadAndSetHostBuilderConfiguration(builder);

            IServiceCollection services = builder.Services;
            IConfiguration configuration = builder.Configuration;

            ConfigureSerilog(builder, initOptions);

            var assemblies = ReflectionHelper.GetAllReferencedAssemblies();
            services.RunBackendModuleInitializers(assemblies);

            ConfigureDbContexts(services, configuration, assemblies);

            ConfigureAuthentication(builder, services, configuration);

            services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(assemblies.ToArray()));

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add<UnitOfWorkFilter>();
                //// Add XML Input Formatter
                //options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
                //// Add XML Output Formatter
                //options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });
            builder.Services.AddControllers().AddXmlDataContractSerializerFormatters();

            ConfigureCors(services, configuration);

            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblies(assemblies);

            services.Configure<JWTOptions>(configuration.GetSection("JWT"));
            services.Configure<IntegrationEventRabbitMQOptions>(configuration.GetSection("RabbitMQ"));
            services.AddEventBus(initOptions.EventBusQueueName, assemblies);
        }

        private static void ConfigureAuthentication(WebApplicationBuilder builder, IServiceCollection services, IConfiguration configuration)
        {
            //services.AddAuthorization(options =>
            //{
            //    // AddPolicy
            //    options.AddPolicy(UserRoles.Administrator, policy => policy.RequireRole(UserRoles.Administrator));
            //});

            services.AddAuthentication();
            var jwtOpt = configuration.GetSection("JWT").Get<JWTOptions>();
            ArgumentNullException.ThrowIfNull(jwtOpt, "JWT");
            services.AddJWTAuthentication(jwtOpt);
            // 【Authorize】Button。
            builder.Services.Configure<SwaggerGenOptions>(c =>
            {
                c.AddAuthenticationHeader();
            });
        }

        private static void ConfigureCors(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                var corsOption = configuration.GetSection("Cors").Get<CorsSettings>();
                ArgumentNullException.ThrowIfNull(corsOption, "Cors");

                options.AddDefaultPolicy(builder => builder.WithOrigins(corsOption.Origins)
                        .AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });
        }

        private static void ConfigureDbContexts(IServiceCollection services, IConfiguration configuration, IEnumerable<Assembly> assemblies)
        {
            // DbContexts
            services.AddAllDbContexts(options =>
            {
                var connectionStrings = configuration.GetValue<string>("DefaultDB:ConnectionStrings");
                ArgumentException.ThrowIfNullOrEmpty(connectionStrings, "DefaultDB:ConnectionStrings");

                options.UseSqlite(connectionStrings);
                options.UseStronglyTypeConverters();
            }, assemblies);
        }

        private static void ConfigureSerilog(WebApplicationBuilder builder, InitializerOptions initOptions)
        {
            //Serilog
            var logFolder = builder.Configuration["LogFolder"];
            if (logFolder == null)
                ArgumentException.ThrowIfNullOrEmpty(logFolder);

            var logFilePath = Path.Combine(logFolder, initOptions.LogFileRelativePath);
            builder.Host.UseSerilog((context, services, configuration) => configuration
                       .ReadFrom.Configuration(context.Configuration)
                       .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                       .ReadFrom.Services(services)
                       .Enrich.FromLogContext()
                       .WriteTo.Console());
        }

        private static void ReadAndSetHostBuilderConfiguration(WebApplicationBuilder builder)
        {
            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string settingsFullPath = Path.Combine(programFilesPath, "Elara\\appsettings.json");
            builder.Configuration.AddJsonFile(settingsFullPath);

            var dbFileName = builder.Configuration.GetValue<string>("DefaultDB:ConnectionStrings");
            ArgumentException.ThrowIfNullOrEmpty(dbFileName, "DefaultDB:ConnectionStrings");

            var dbFileFolder = Path.Combine(programFilesPath, "Elara\\db");//C:\Program Files\Elara\db
            Directory.CreateDirectory(dbFileFolder);

            var dbFullPath = Path.Combine(dbFileFolder, dbFileName);
            builder.Configuration["DefaultDB:ConnectionStrings"] = "Data Source=" + dbFullPath;
            builder.Configuration["LogFolder"] = Path.Combine(programFilesPath, "Elara\\logs");//C:\Program Files\Elara\logs
        }
    }
}