using Commons.Helpers;
using FileService.Domain.Entities;

namespace FileService.Domain;

public class FileServiceDomainService
{
    private readonly IFileServiceRepository repository;
    private readonly IStorageClient backupStorage;//备份服务器
    private readonly IStorageClient remoteStorage;//文件存储服务器

    public FileServiceDomainService(IFileServiceRepository repository,
        IEnumerable<IStorageClient> storageClients)
    {
        this.repository = repository;

        this.backupStorage = storageClients.First(c => c.StorageType == StorageType.Backup);
        this.remoteStorage = storageClients.First(c => c.StorageType == StorageType.Public);
    }

    public async Task<UploadedItemResult> UploadAsync(Stream stream, string fileName, CancellationToken cancellationToken)
    {
        string hash = HashHelper.ComputeSha256Hash(stream);
        long fileSize = stream.Length;
        DateTime today = DateTime.Today;

        string partialPath = $"{today.Year}/{today.Month}/{today.Day}/{hash}/{fileName}";

        var oldUploadItem = await repository.FindFileAsync(fileSize, hash);
        if (oldUploadItem is not null)
        {
            return new UploadedItemResult(true, oldUploadItem);
        }
        stream.Position = 0;
        Uri backupUrl = await backupStorage.SaveAsync(partialPath, stream, cancellationToken);//保存到本地备份
        stream.Position = 0;
        Uri remoteUrl = await remoteStorage.SaveAsync(partialPath, stream, cancellationToken);//保存到生产的存储系统
        stream.Position = 0;
        return new UploadedItemResult(false, new UploadedItem(fileSize, fileName, hash, backupUrl, remoteUrl));

    }
}