namespace ApiClients.Abstractions.ChatApiClient.ReplyMessage.Requests;
public record ReplyMessageRequest(Guid MessageId, Guid ConversationId, string Content, Guid[] MessageAttachmentIds, Guid? QuoteMessage);
