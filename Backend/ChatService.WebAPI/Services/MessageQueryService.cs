using ChatService.Domain;
using ChatService.Domain.Entities;
using ChatService.WebAPI.Models.Responses;
using DomainCommons.EntityStronglyIds;
using Profile;
using UploadedItem;

namespace ChatService.WebAPI.Services;

public class MessageQueryService : IMessageQueryService
{
    private readonly IChatServiceRepository repository;
    private readonly ProfileService.ProfileServiceClient profileServiceClient;
    private readonly UploadedItemService.UploadedItemServiceClient uploadedItemServiceClient;
    private readonly IHttpContextAccessor httpContextAccessor;

    public MessageQueryService(IChatServiceRepository repository,
                               ProfileService.ProfileServiceClient profileServiceClient,
                               UploadedItemService.UploadedItemServiceClient uploadedItemServiceClient,
                               IHttpContextAccessor httpContextAccessor)
    {
        this.repository = repository;
        this.profileServiceClient = profileServiceClient;
        this.uploadedItemServiceClient = uploadedItemServiceClient;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<MessageWithReplyMessageResponse[]> GetMessagesWithReplyMessagesAsync(Message[] messages)
    {
        var result = new List<MessageWithReplyMessageResponse>();

        foreach (var message in messages)
        {
            var messageResponse = await GetMessageWithReplyMessageAsync(message.Id);
            if (messageResponse is not null)
            {
                result.Add(messageResponse);
            }
        }

        return [.. result];
    }

    public async Task<MessageWithReplyMessageResponse[]> GetMessagesWithReplyMessagesAsync(MessageId[] ids)
    {
        var messages = await repository.FindMessagesByIdsAsync(ids);

        return await GetMessagesWithReplyMessagesAsync(messages);
    }

    public async Task<MessageWithReplyMessageResponse?> GetMessageWithReplyMessageAsync(MessageId id)
    {
        var message = await repository.FindMessageByIdAsync(id);
        if (message is null)
            return null;

        var senderInfo = await GetUserInfoAsync(message.SenderId);
        if (senderInfo is null)
            return null;

        var unreadMessages = await repository.GetUserUnreadMessagesAsync(GetCurrentUserId());
        var unreadIds = unreadMessages.Select(x => x.MessageId).ToHashSet();

        var attachments = await GetMessageAttachmentsAsync(message.Id);

        QuoteMessageResponse? quoteMessage = null;
        if (message.QuoteMessageId is not null)
        {
            quoteMessage = await GetQuoteMessageResponseAsync(message.QuoteMessageId.Value);
        }

        var replyMessages = await GetReplyMessagesAsync(message.Id);

        return new MessageWithReplyMessageResponse
        {
            IsUnread = unreadIds.Contains(message.Id),
            MessageId = message.Id,
            ConversationId = message.ConversationId,
            QuoteMessage = quoteMessage,
            RelayMessages = replyMessages,
            Content = message.Content,
            Sender = senderInfo,
            Attachments = attachments,
            SendAt = message.CreatedAt,
            UpdatedAt = message.UpdatedAt
        };
    }

    public async Task<QuoteMessageResponse?> GetQuoteMessageResponseAsync(MessageId id)
    {
        var message = await repository.FindMessageByIdAsync(id);
        if (message is null)
            return null;

        var sender = await GetUserInfoAsync(message.SenderId);
        if (sender is null)
            return null;

        var uploadedItems = await GetMessageAttachmentsAsync(message.Id);

        return new QuoteMessageResponse
        {
            ConversationId = message.ConversationId,
            MessageId = message.Id,
            Sender = sender,
            Content = message.Content,
            Attachments = uploadedItems,
            SendAt = message.CreatedAt,
            UpdatedAt = message.UpdatedAt
        };
    }

    public async Task<ReplyMessageResponse[]> GetReplyMessagesAsync(MessageId id)
    {
        var replyMessages = await repository.MessageAllReplyMessagesAsync(id);
        if (!replyMessages.Any())
            return [];

        var result = new List<ReplyMessageResponse>();
        var unreadIds = (await repository.GetUserUnreadMessagesAsync(GetCurrentUserId()))
                        .Select(x => x.MessageId).ToHashSet();

        foreach (var reply in replyMessages)
        {
            var message = await repository.FindMessageByIdAsync(reply.OriginalMessageId);
            if (message is null)
                continue;

            var sender = await GetUserInfoAsync(message.SenderId);
            if (sender is null)
                continue;

            var attachments = await GetMessageAttachmentsAsync(message.Id);

            result.Add(new ReplyMessageResponse
            {
                IsUnread = unreadIds.Contains(message.Id),
                MessageId = message.Id,
                Sender = sender,
                ConversationId = message.ConversationId,
                Content = message.Content,
                Attachments = attachments,
                SendAt = message.CreatedAt,
                UpdatedAt = message.UpdatedAt
            });
        }

        return [.. result];
    }

    public async Task<UploadedItemResponse[]> GetMessageAttachmentsAsync(MessageId messageId)
    {
        var attachments = await repository.GetMessageAllMessageAttachmentsAsync(messageId);
        var uploadedItemIds = attachments.Select(x => x.UploadedItemId).ToList();

        var result = new List<UploadedItemResponse>();
        if (uploadedItemIds.Any())
        {
            var itemsRequest = new UploadedItemsRequest();
            itemsRequest.Ids.AddRange(uploadedItemIds.Select(x => x.ToString()));

            var reply = uploadedItemServiceClient.GetUploadedItems(itemsRequest);

            foreach (var item in reply.Items)
            {
                result.Add(new UploadedItemResponse
                {
                    Id = UploadedItemId.Parse(item.Id),
                    Filename = item.Filename,
                    FileType = item.FileType,
                    FileSizeInBytes = item.FileSizeInBytes,
                    FileSHA256Hash = item.FileSha256Hash,
                    Url = new Uri(item.Url),
                    UploadedAt = DateTimeOffset.Parse(item.UploadedAt)
                });
            }
        }

        return [.. result];
    }

    public async Task<UserInfoResponse?> GetUserInfoAsync(UserId senderId)
    {
        var reply = await profileServiceClient.GetUserProfileAsync(new GetUserProfileRequest { UserId = senderId.ToString() });
        if (reply.UserId != senderId.ToString())
            return null;

        UploadedItemResponse? avatar = null;

        if (UploadedItemId.TryParse(reply.AvatarId, out var avatarId))
        {
            var avatarReply = uploadedItemServiceClient.GetUploadedItem(new UploadedItemRequest { Id = avatarId.ToString() });
            if (avatarReply.Id == avatarId.ToString())
            {
                avatar = new UploadedItemResponse
                {
                    Id = avatarId,
                    Filename = avatarReply.Filename,
                    FileType = avatarReply.FileType,
                    FileSizeInBytes = avatarReply.FileSizeInBytes,
                    FileSHA256Hash = avatarReply.FileSha256Hash,
                    Url = new Uri(avatarReply.Url),
                    UploadedAt = DateTimeOffset.Parse(avatarReply.UploadedAt)
                };
            }
        }

        return new UserInfoResponse
        {
            UserId = senderId,
            DisplayName = reply.DisplayName,
            Avatar = avatar
        };
    }

    private UserId GetCurrentUserId()
    {
        var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        return userId is null ? throw new UnauthorizedAccessException("User not authenticated") : UserId.Parse(userId);
    }
}
