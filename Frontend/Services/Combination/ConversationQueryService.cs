using Services.Abstractions;
using Services.Abstractions.ChatServices;
using Services.Abstractions.PersonalSpaceServices;
using Services.Abstractions.Results;
using Services.Abstractions.Results.Data;

namespace Services.Combination;

public class ConversationQueryService : IConversationQueryService
{
    private readonly IChatConversationService conversationService;
    private readonly IChatMessageService messageService;
    private readonly IPersonalSpaceProfileService profileService;
    private readonly IFileService fileService;

    public ConversationQueryService(IChatConversationService conversationService, IChatMessageService chatMessageService, IPersonalSpaceProfileService profileService, IFileService fileService)
    {
        this.conversationService = conversationService;
        this.messageService = chatMessageService;
        this.profileService = profileService;
        this.fileService = fileService;
    }

    public async Task<ApiServiceResult<ConversationData[]>> GetConversationWithMessagesAsync(CancellationToken cancellationToken = default)
    {
        var conversationsResult = await conversationService.GetUserConversationsAsync();
        if (!conversationsResult.IsSuccessful)
            return ApiServiceResult<ConversationData[]>.FromFailure(conversationsResult);

        var conversations = new List<ConversationData>();

        foreach (var data in conversationsResult.ResultData)
        {
            var conversationMessages = await GetConversationMessagesAsync(data.Id, cancellationToken);
            if (!conversationMessages.IsSuccessful)
                return ApiServiceResult<ConversationData[]>.FromFailure(conversationMessages);

            var conversation = new ConversationData()
            {
                Id = data.Id,
                Messages = conversationMessages.ResultData,
                Name = data.Name
            };

            conversations.Add(conversation);
        }

        return new ApiServiceResult<ConversationData[]>()
        {
            IsSuccessful = true,
            ResultData = conversations.ToArray()
        };
    }

    public async Task<ApiServiceResult<MessageData[]>> GetConversationMessagesAsync(Guid conversationId, CancellationToken cancellationToken = default)
    {
        var messagesResult = await conversationService.GetConversationMessagesAsync(conversationId);
        if (!messagesResult.IsSuccessful)
            return ApiServiceResult<MessageData[]>.FromFailure(messagesResult);

        var messages = new List<MessageData>();

        foreach (var data in messagesResult.ResultData)
        {
            var replyMessages = await GetMessageReplyMessagesAsync(data.Id, cancellationToken);
            if (!replyMessages.IsSuccessful)
                return ApiServiceResult<MessageData[]>.FromFailure(replyMessages);

            var senderDataResult = await GetSenderDataById(data.SenderId, cancellationToken);
            if (!senderDataResult.IsSuccessful)
                return ApiServiceResult<MessageData[]>.FromFailure(senderDataResult);

            var sender = senderDataResult.ResultData;

            var uploadedItemResult = await GetUploadedItemByIds(data.UploadedItemIds, cancellationToken);
            if (!uploadedItemResult.IsSuccessful)
                return ApiServiceResult<MessageData[]>.FromFailure(uploadedItemResult);

            var uploadedItems = uploadedItemResult.ResultData;

            QuoteMessageData? quoteMessage = null;
            if (data.QuoteMessageId is not null)
            {
                var quoteMessageResult = await GetQuoteMessageAsync(data.QuoteMessageId.Value, cancellationToken);
                if (!quoteMessageResult.IsSuccessful)
                    return ApiServiceResult<MessageData[]>.FromFailure(quoteMessageResult);

                quoteMessage = quoteMessageResult.ResultData;
            }

            var message = new MessageData()
            {
                ConversationId = data.ConversationId,
                Id = data.Id,
                IsUnread = data.IsUnread,
                Content = data.Content,
                Sender = sender,
                ReplyMessages = replyMessages.ResultData,
                UploadedItems = uploadedItems,
                QuoteMessage = quoteMessage,
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
            };
            messages.Add(message);
        }
        return new ApiServiceResult<MessageData[]>()
        {
            IsSuccessful = true,
            ResultData = messages.ToArray()
        };
    }

    public async Task<ApiServiceResult<ReplyMessageData[]>> GetMessageReplyMessagesAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        var replyMessagesResult = await messageService.GetReplyMessagesAsync(messageId);
        if (!replyMessagesResult.IsSuccessful)
            return ApiServiceResult<ReplyMessageData[]>.FromFailure(replyMessagesResult);

        var replyMessages = new List<ReplyMessageData>();

        var replyMessageData = replyMessagesResult.ResultData;

        foreach (var data in replyMessageData)
        {
            var senderDataResult = await GetSenderDataById(data.SenderId, cancellationToken);
            if (!senderDataResult.IsSuccessful)
                return ApiServiceResult<ReplyMessageData[]>.FromFailure(senderDataResult);

            var sender = senderDataResult.ResultData;

            var uploadedItemResult = await GetUploadedItemByIds(data.UploadedItemIds, cancellationToken);
            if (!uploadedItemResult.IsSuccessful)
                return ApiServiceResult<ReplyMessageData[]>.FromFailure(uploadedItemResult);

            var uploadedItems = uploadedItemResult.ResultData;

            QuoteMessageData? quoteMessage = null;
            if (data.QuoteMessageId is not null)
            {
                var quoteMessageResult = await GetQuoteMessageAsync(data.QuoteMessageId.Value, cancellationToken);
                if (!quoteMessageResult.IsSuccessful)
                    return ApiServiceResult<ReplyMessageData[]>.FromFailure(quoteMessageResult);

                quoteMessage = quoteMessageResult.ResultData;
            }

            var replyMessage = new ReplyMessageData()
            {
                MessageId = data.Id,
                IsUnread = data.IsUnread,
                Content = data.Content,
                Sender = sender,
                UploadedItems = uploadedItems,
                QuoteMessage = quoteMessage,
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
            };
            replyMessages.Add(replyMessage);
        }

        return new ApiServiceResult<ReplyMessageData[]>()
        {
            IsSuccessful = true,
            ResultData = replyMessages.ToArray()
        };
    }

    public async Task<ApiServiceSimpleResult<QuoteMessageData>> GetQuoteMessageAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        var messagesResult = await messageService.FindByIdAsync(messageId);
        if (!messagesResult.IsSuccessful)
        {
            if (!messagesResult.IsServerError)
            {
                return new ApiServiceSimpleResult<QuoteMessageData>()
                {
                    IsSuccessful = true,
                };
            }

            return ApiServiceSimpleResult<QuoteMessageData>.FromFailure(messagesResult);
        }

        var senderDataResult = await GetSenderDataById(messagesResult.ResultData.SenderId, cancellationToken);
        if (!senderDataResult.IsSuccessful)
            return ApiServiceSimpleResult<QuoteMessageData>.FromFailure(senderDataResult);

        var sender = senderDataResult.ResultData;

        var uploadedItemResult = await GetUploadedItemByIds(messagesResult.ResultData.UploadedItemIds, cancellationToken);
        if (!uploadedItemResult.IsSuccessful)
            return ApiServiceSimpleResult<QuoteMessageData>.FromFailure(senderDataResult);

        var data = new QuoteMessageData()
        {
            MessageId = messagesResult.ResultData.Id,
            Content = messagesResult.ResultData.Content,
            Sender = sender,
            UploadedItems = uploadedItemResult.ResultData,
            CreatedAt = messagesResult.ResultData.CreatedAt,
            UpdatedAt = messagesResult.ResultData.UpdatedAt,
        };

        return new ApiServiceSimpleResult<QuoteMessageData>()
        {
            IsSuccessful = true,
            ResultData = data
        };
    }

    public async Task<ApiServiceResult<MessageSenderData>> GetSenderDataById(Guid senderId, CancellationToken cancellationToken = default)
    {
        var profileResult = await profileService.GetUserProfileAsync(senderId, cancellationToken);
        if (!profileResult.IsSuccessful)
            return ApiServiceResult<MessageSenderData>.FromFailure(profileResult);

        var avatarItemResult = await fileService.GetFileItemAsync(profileResult.ResultData.AvatarItemId, cancellationToken);
        if (!avatarItemResult.IsSuccessful)
            return ApiServiceResult<MessageSenderData>.FromFailure(profileResult);

        var avatarItem = new UploadedItemData() { Id = avatarItemResult.ResultData.Id, Uri = avatarItemResult.ResultData.RemoteUrl };

        var sender = new MessageSenderData()
        {
            SenderId = profileResult.ResultData.UserId,
            Name = profileResult.ResultData.DisplayName,
            Avatar = avatarItem,
        };

        return new ApiServiceResult<MessageSenderData>()
        {
            IsSuccessful = true,
            IsServerError = false,
            ResultData = sender,
        };
    }

    public async Task<ApiServiceResult<UploadedItemData[]>> GetUploadedItemByIds(Guid[] fileIds, CancellationToken cancellationToken = default)
    {
        var uploadedItemResult = await fileService.GetFileItemsAsync(fileIds, cancellationToken);
        if (!uploadedItemResult.IsSuccessful)
            return ApiServiceResult<UploadedItemData[]>.FromFailure(uploadedItemResult);

        var uploadedItems = uploadedItemResult.ResultData.Select(x =>
        {
            return new UploadedItemData() { Id = x.Id, Uri = x.RemoteUrl };
        }).ToArray();

        return new ApiServiceResult<UploadedItemData[]>()
        {
            IsSuccessful = true,
            ResultData = uploadedItems,
        };
    }
}
