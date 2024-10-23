using FileService.Domain.Entities;

namespace FileService.Domain
{
    public interface IFileServiceRepository
    {

        Task<UploadedItem?> FindFileAsync(long fileSize, string sha256Hash);
    }
}