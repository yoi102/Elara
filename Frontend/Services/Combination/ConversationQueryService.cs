using DataProviders.Abstractions;
using Services.Abstractions;
using Services.Abstractions.ChatServices;
using Services.Abstractions.PersonalSpaceServices;
using Services.Abstractions.Results;
using Services.Abstractions.Results.Data;

namespace Services.Combination;

public class ConversationQueryService : IConversationQueryService
{
    private readonly IChatConversationService conversationService;
    private readonly IFileService fileService;
    private readonly IUserProfileService userProfileService;
    private readonly IChatMessageService messageService;
    private readonly IUserDataProvider userDataProvider;

    //TODO：感觉会非常耗时。。。。。。先这样把、、迟点并发优化
    public ConversationQueryService(IUserDataProvider userDataProvider, IChatConversationService conversationService, IChatMessageService chatMessageService, IPersonalSpaceProfileService profileService, IFileService fileService, IUserProfileService userProfileService)
    {
        this.userDataProvider = userDataProvider;
        this.conversationService = conversationService;
        this.messageService = chatMessageService;
        this.fileService = fileService;
        this.userProfileService = userProfileService;
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

            var senderDataResult = await userProfileService.GetUserInfoDataById(data.SenderId, cancellationToken);
            if (!senderDataResult.IsSuccessful)
                return ApiServiceResult<MessageData[]>.FromFailure(senderDataResult);

            var sender = senderDataResult.ResultData;

            var fileItemsResult = await fileService.GetFileItemsAsync(data.UploadedItemIds, cancellationToken);
            if (!fileItemsResult.IsSuccessful)
                return ApiServiceResult<MessageData[]>.FromFailure(fileItemsResult);

            var fileItems = fileItemsResult.ResultData;

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
                Attachments = fileItems,
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

    public async Task<ApiServiceResult<ParticipantData[]>> GetConversationParticipantsAsync(Guid conversationId, CancellationToken cancellationToken = default)
    {
        var participantsResult = await conversationService.GetConversationParticipantsAsync(conversationId, cancellationToken);
        if (!participantsResult.IsSuccessful)
            return ApiServiceResult<ParticipantData[]>.FromFailure(participantsResult);
        var participants = new List<ParticipantData>();

        foreach (var data in participantsResult.ResultData)
        {
            var userDataResult = await userProfileService.GetUserInfoDataById(data.UserId, cancellationToken);
            if (!userDataResult.IsSuccessful)
                return ApiServiceResult<ParticipantData[]>.FromFailure(userDataResult);

            var participant = new ParticipantData()
            {
                UserInfoData = userDataResult.ResultData,
                Role = data.Role
            };
            participants.Add(participant);
        }

        return new ApiServiceResult<ParticipantData[]>()
        {
            IsSuccessful = true,
            ResultData = participants.ToArray()
        };
    }

    public async Task<ApiServiceResult<ConversationData[]>> GetConversationsWithMessagesAsync(CancellationToken cancellationToken = default)
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

            var participantsResult = await GetConversationParticipantsAsync(data.Id, cancellationToken);
            if (!participantsResult.IsSuccessful)
                return ApiServiceResult<ConversationData[]>.FromFailure(participantsResult);
            string conversationName;
            if (data.IsGroup)
            {
                conversationName = data.Name;
            }
            else
            {
                var other = participantsResult.ResultData.Where(p => p.UserInfoData.UserId != userDataProvider.UserId).Single();
                conversationName = other.UserInfoData.DisplayName;
            }

            var conversation = new ConversationData()
            {
                Id = data.Id,
                Messages = conversationMessages.ResultData,
                IsGroup = data.IsGroup,
                Name = conversationName,
                Participants = participantsResult.ResultData,
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt
            };

            conversations.Add(conversation);
        }

        return new ApiServiceResult<ConversationData[]>()
        {
            IsSuccessful = true,
            ResultData = conversations.ToArray()
        };
    }

    public async Task<ApiServiceResult<MessageData[]>> GetConversationUnreadMessagesAsync(Guid conversationId, CancellationToken cancellationToken = default)
    {
        var unreadMessagesResult = await conversationService.GetUnreadMessagesAsync(conversationId);
        if (!unreadMessagesResult.IsSuccessful)
            return ApiServiceResult<MessageData[]>.FromFailure(unreadMessagesResult);

        var messages = new List<MessageData>();

        foreach (var data in unreadMessagesResult.ResultData)
        {
            var replyMessages = await GetMessageReplyMessagesAsync(data.Id, cancellationToken);
            if (!replyMessages.IsSuccessful)
                return ApiServiceResult<MessageData[]>.FromFailure(replyMessages);

            var senderDataResult = await userProfileService.GetUserInfoDataById(data.SenderId, cancellationToken);
            if (!senderDataResult.IsSuccessful)
                return ApiServiceResult<MessageData[]>.FromFailure(senderDataResult);

            var sender = senderDataResult.ResultData;

            var fileItemsResult = await fileService.GetFileItemsAsync(data.UploadedItemIds, cancellationToken);
            if (!fileItemsResult.IsSuccessful)
                return ApiServiceResult<MessageData[]>.FromFailure(fileItemsResult);

            var fileItems = fileItemsResult.ResultData;

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
                Attachments = fileItems,
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
            var senderDataResult = await userProfileService.GetUserInfoDataById(data.SenderId, cancellationToken);
            if (!senderDataResult.IsSuccessful)
                return ApiServiceResult<ReplyMessageData[]>.FromFailure(senderDataResult);

            var sender = senderDataResult.ResultData;

            var fileItemsResult = await fileService.GetFileItemsAsync(data.UploadedItemIds, cancellationToken);
            if (!fileItemsResult.IsSuccessful)
                return ApiServiceResult<ReplyMessageData[]>.FromFailure(fileItemsResult);

            var fileItems = fileItemsResult.ResultData;

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
                FileItems = fileItems,
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

        var senderDataResult = await userProfileService.GetUserInfoDataById(messagesResult.ResultData.SenderId, cancellationToken);
        if (!senderDataResult.IsSuccessful)
            return ApiServiceSimpleResult<QuoteMessageData>.FromFailure(senderDataResult);

        var sender = senderDataResult.ResultData;

        var fileItemsResult = await fileService.GetFileItemsAsync(messagesResult.ResultData.UploadedItemIds, cancellationToken);
        if (!fileItemsResult.IsSuccessful)
            return ApiServiceSimpleResult<QuoteMessageData>.FromFailure(senderDataResult);

        var data = new QuoteMessageData()
        {
            MessageId = messagesResult.ResultData.Id,
            Content = messagesResult.ResultData.Content,
            Sender = sender,
            FileItems = fileItemsResult.ResultData,
            CreatedAt = messagesResult.ResultData.CreatedAt,
            UpdatedAt = messagesResult.ResultData.UpdatedAt,
        };

        return new ApiServiceSimpleResult<QuoteMessageData>()
        {
            IsSuccessful = true,
            ResultData = data
        };
    }
}
