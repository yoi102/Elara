using DomainCommons.EntityStronglyIds;
using FileService.Domain.Entities;

namespace FileService.Domain;

public interface IFileServiceRepository
{
    Task<UploadedItem?> FindFileAsync(long fileSize, string sha256Hash);
    Task<UploadedItem?> GetFileByIdAsync(UploadedItemId id);
    Task<UploadedItem[]> GetFilesByIdsAsync(UploadedItemId[] ids);
    Task<UploadedItem[]> GetUploadedItemsAsync(UploadedItemId[] ids);
}
