using Commons.Interfaces;
using FileService.Domain;
using FileService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.Infrastructure;

internal class ModuleInitializer : IBackendModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<IStorageClient, SMBStorageClient>();
        services.AddScoped<IStorageClient, MockCloudStorageClient>();
        services.AddScoped<IFileServiceRepository, FileServiceRepository>();
        services.AddScoped<FileServiceDomainService>();
    }
}
