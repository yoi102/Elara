using ChatService.Domain.Entities;
using ChatService.WebAPI.Models.Responses;
using DomainCommons.EntityStronglyIds;

namespace ChatService.WebAPI.Services;

public interface IMessageQueryService
{
    Task<MessageWithReplyMessageResponse[]> GetMessagesWithReplyMessagesAsync(Message[] messages);

    Task<MessageWithReplyMessageResponse[]> GetMessagesWithReplyMessagesAsync(MessageId[] ids);

    Task<MessageWithReplyMessageResponse?> GetMessageWithReplyMessageAsync(MessageId id);

    Task<QuoteMessageResponse?> GetQuoteMessageResponseAsync(MessageId id);

    Task<ReplyMessageResponse[]> GetReplyMessagesAsync(MessageId id);

    Task<UploadedItemResponse[]> GetMessageAttachmentsAsync(MessageId messageId);

    Task<UserInfoResponse?> GetUserInfoAsync(UserId senderId);
}
