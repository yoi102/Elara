using DomainCommons.EntityStronglyIds;
using FileService.Domain;
using Grpc.Core;
using UploadedItem;

namespace FileService.WebAPI.Services;

public class UploadedItemService : UploadedItem.UploadedItemService.UploadedItemServiceBase
{
    private readonly ILogger<UploadedItemService> logger;
    private readonly IFileServiceRepository fileServiceRepository;

    public UploadedItemService(ILogger<UploadedItemService> logger, IFileServiceRepository fileServiceRepository)
    {
        this.logger = logger;
        this.fileServiceRepository = fileServiceRepository;
    }

    public override async Task<UploadedItemReply> GetUploadedItem(UploadedItemRequest request, ServerCallContext context)
    {
        if (!UploadedItemId.TryParse(request.Id, out var fileId))
        {
            return new UploadedItemReply();
        }

        var fileItem = await fileServiceRepository.GetFileByIdAsync(fileId);

        if (fileItem == null) return new UploadedItemReply();

        return new UploadedItemReply
        {
            Id = fileItem.Id.ToString(),
            Filename = fileItem.Filename,
            FileSha256Hash = fileItem.FileSHA256Hash,
            FileSizeInBytes = fileItem.FileSizeInBytes,
            FileType = fileItem.FileType,
            UploadedAt = fileItem.CreatedAt.ToString(),
            Url = fileItem.RemoteUrl.ToString()
        };
    }

    public override async Task<UploadedItemsReply> GetUploadedItems(UploadedItemsRequest request, ServerCallContext context)
    {
        var fileIds = request.Ids.Select(x => UploadedItemId.TryParse(x, out var id) ? id : UploadedItemId.Empty)
                                               .Where(id => id != UploadedItemId.Empty).ToArray();

        var fileItems = await fileServiceRepository.GetFilesByIdsAsync(fileIds);

        if (fileItems == null || fileItems.Length == 0)
        {
            return new UploadedItemsReply();
        }
        var itemsReply = new UploadedItemsReply();
        foreach (var fileItem in fileItems)
        {
            var reply = new UploadedItemReply
            {
                Id = fileItem.Id.ToString(),
                Filename = fileItem.Filename,
                FileSha256Hash = fileItem.FileSHA256Hash,
                FileSizeInBytes = fileItem.FileSizeInBytes,
                FileType = fileItem.FileType,
                UploadedAt = fileItem.CreatedAt.ToString(),
                Url = fileItem.RemoteUrl.ToString()
            };
            itemsReply.Items.Add(reply);
        }

        return itemsReply;
    }
}
