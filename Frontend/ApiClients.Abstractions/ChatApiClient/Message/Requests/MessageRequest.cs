namespace ApiClients.Abstractions.ChatApiClient.Message.Requests;

public record MessageRequest(Guid ConversationId, string Content, Guid[] MessageAttachmentIds, Guid? QuoteMessage);
