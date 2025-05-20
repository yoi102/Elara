using DomainCommons.EntityStronglyIds;

namespace FileService.WebAPI.Controllers.Uploader.Models;

public record UploadedItemResponse(UploadedItemId Id, long FileSizeInBytes, string Filename, string FileType, string FileSHA256Hash, Uri Url, DateTimeOffset UploadedAt);
