using DomainCommons.EntityStronglyIds;
using FileService.Domain;
using Grpc.Core;
using UploadedItem;

namespace FileService.WebAPI.Services;

public class UploadedItemService : UploadedItem.UploadedItem.UploadedItemBase
{
    private readonly ILogger<UploadedItemService> logger;
    private readonly FileServiceDomainService domainService;

    public UploadedItemService(ILogger<UploadedItemService> logger, FileServiceDomainService domainService)
    {
        this.logger = logger;
        this.domainService = domainService;
    }

    public override async Task<UploadedItemReply> GetUploadedItems(UploadedItemRequest request, ServerCallContext context)
    {
        var itemIds = request.Ids
                                        .Select(id => UploadedItemId.TryParse(id, out var parsedId) ? parsedId : (UploadedItemId?)null)
                                        .Where(parsedId => parsedId != null)
                                        .Select(parsedId => parsedId!.Value).ToArray();

        var items = await domainService.GetFilesAsync(itemIds);

        var reply = new UploadedItemReply();
        reply.Urls.AddRange(items);

        return reply;
    }
}
