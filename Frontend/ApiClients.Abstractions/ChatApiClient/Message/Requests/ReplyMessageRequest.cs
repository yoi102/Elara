namespace ApiClients.Abstractions.ChatApiClient.Message.Requests;
public record ReplyMessageRequest(Guid MessageId, Guid ConversationId, string Content, Guid[] MessageAttachmentIds, Guid? QuoteMessage);
