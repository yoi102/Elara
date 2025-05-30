﻿using ChatService.Domain.Entities;
using DomainCommons.EntityStronglyIds;
using DomainCommons.Enums;

namespace ChatService.Domain;

public interface IChatServiceRepository
{
    #region Conversation

    Task<Conversation?> FindConversationByIdAsync(ConversationId id);

    Task<Conversation[]> GetConversationsByUserIdAsync(UserId id);

    Task<Message?> GetLatestMessage(ConversationId id);

    Task<Message[]> GetMessagesBefore(ConversationId id, DateTimeOffset before);

    Task<UserUnreadMessage[]> GetUnreadMessagesAsync(UserId userId, ConversationId conversationId);

    Task<UserUnreadMessage[]> GetUserUnreadMessagesAsync(UserId userId);

    #endregion Conversation

    #region Participant

    Task<Participant?> FindParticipantByIdAsync(ParticipantId id);

    Task<Participant[]> GetConversationParticipantsAsync(ConversationId id);

    #endregion Participant

    #region Message

    Task<Message?> FindMessageByIdAsync(MessageId id);

    Task<Message[]> FindMessagesByIdsAsync(MessageId[] ids);

    Task<Message[]> GetConversationAllMessagesAsync(ConversationId id);

    Task<Message?> UpdateMessageAsync(MessageId messageId, string content, UploadedItemId[] attachments, MessageId? quoteMessage);

    #endregion Message

    #region MessageAttachment

    Task<MessageAttachment[]> GetMessageAllMessageAttachmentsAsync(MessageId id);

    #endregion MessageAttachment

    #region ReplyMessage

    Task<ReplyMessage?> FindReplyMessageByIdAsync(MessageId id);

    Task<ReplyMessage?> GetLatestReplyMessage(MessageId id);

    Task<ReplyMessage[]> MessageAllReplyMessagesAsync(MessageId id);

    #endregion ReplyMessage

    #region ConversationRequest

    Task<ConversationRequest> CreateConversationRequestAsync(UserId senderId, UserId receiverId, ConversationId conversationId, string role);

    Task<ConversationRequest?> FindConversationRequestByIdAsync(ConversationRequestId conversationRequestId);

    Task<ConversationRequest[]> GetAllReceiverConversationRequestAsync(UserId receiverId);

    Task<ConversationRequest[]> GetPendingConversationRequestByReceiverIdAsync(UserId receiverId);

    Task<ConversationRequest?> UpdateConversationRequestAsync(ConversationRequestId conversationRequestId, RequestStatus status);

    #endregion ConversationRequest
}
