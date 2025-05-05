namespace ApiClients.Abstractions.ChatApiClient.Conversation.Requests;
public record ConversationMemberRequest(Guid UserId, string Role);

