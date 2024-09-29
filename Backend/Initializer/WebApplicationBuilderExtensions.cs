using ASPNETCore;
using Commons;
using EventBus;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.EFCore;
using JWT;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;


namespace Initializer
{
    public static class WebApplicationBuilderExtensions
    {
        public static void ReadAndSetHostBuilderConfiguration(this WebApplicationBuilder builder)
        {

            string settingsFullPath = Path.Combine("C:\\Users\\yoiri\\source\\repos\\Elara", "appsettings.json");
            builder.Configuration.AddJsonFile(settingsFullPath);

            var dbFileName = builder.Configuration.GetValue<string>("DefaultDB:ConnectionStrings");
            ArgumentException.ThrowIfNullOrEmpty(dbFileName, "DefaultDB:ConnectionStrings");

            string programFilesFolder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var dbFileFolder = Path.Combine(programFilesFolder, "Elara\\db");
            Directory.CreateDirectory(dbFileFolder);

            var dbFullPath = Path.Combine(dbFileFolder, dbFileName);
            builder.Configuration["DefaultDB:ConnectionStrings"] = "Data Source=" + dbFullPath;
            builder.Configuration["LogFolder"] = Path.Combine(programFilesFolder, "Elara\\logs");
        }

        public static void ConfigureCommonServices(this WebApplicationBuilder builder, InitializerOptions initOptions)
        {
            IServiceCollection services = builder.Services;
            IConfiguration configuration = builder.Configuration;

            ConfigureSerilog(builder, initOptions);

            var assemblies = ReflectionHelper.GetAllReferencedAssemblies();
            services.RunModuleInitializers(assemblies);


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

            ConfigureRedis(services, configuration);

        }

        private static void ConfigureRedis(IServiceCollection services, IConfiguration configuration)
        {
            //Redis的配置
            var redisConfiguration = configuration.GetValue<string>("Redis:ConnectionStrings");
            ArgumentException.ThrowIfNullOrEmpty(redisConfiguration, "Redis:ConnectionStrings");

            IConnectionMultiplexer redisConnMultiplexer = ConnectionMultiplexer.Connect(redisConfiguration);
            services.AddSingleton(typeof(IConnectionMultiplexer), redisConnMultiplexer);
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
            });
        }

        private static void ConfigureCors(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                var corsOpt = configuration.GetSection("Cors").Get<CorsSettings>();
                ArgumentNullException.ThrowIfNull(corsOpt, "Cors");

                options.AddDefaultPolicy(builder => builder.WithOrigins(corsOpt.Origins)
                        .AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });
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

        private static void ConfigureDbContexts(IServiceCollection services, IConfiguration configuration, IEnumerable<Assembly> assemblies)
        {
            // DbContexts
            services.AddAllDbContexts(options =>
            {
                //options.UseStronglyTypeConverters();
                var connectionStrings = configuration.GetValue<string>("DefaultDB:ConnectionStrings");
                ArgumentException.ThrowIfNullOrEmpty(connectionStrings, "DefaultDB:ConnectionStrings");

                options.UseSqlite(connectionStrings);
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
    }
}