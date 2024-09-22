using DomainCommons.EntityStronglyIds;
using FileService.Domain;
using FileService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileService.Infrastructure;

internal class FileServiceRepository : IFileServiceRepository
{
    private readonly FileServiceDbContext dbContext;

    public FileServiceRepository(FileServiceDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<UploadedItem?> FindFileAsync(long fileSize, string sha256Hash)
    {
        return await dbContext.UploadItems.FirstOrDefaultAsync(u => u.FileSizeInBytes == fileSize
        && u.FileSHA256Hash == sha256Hash);
    }

    public async Task<UploadedItem?> GetFileByIdAsync(UploadedItemId id)
    {
        return await dbContext.UploadItems.FindAsync(id);
    }

    public async Task<UploadedItem[]> GetFilesByIdsAsync(UploadedItemId[] ids)
    {
        return await dbContext.UploadItems.Where(x => ids.Contains(x.Id)).ToArrayAsync();
    }

    public async Task<UploadedItem[]> GetUploadedItemsAsync(UploadedItemId[] ids)
    {
        return await dbContext.UploadItems.Where(u => ids.Contains(u.Id)).ToArrayAsync();
    }
}
