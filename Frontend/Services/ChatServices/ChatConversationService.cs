using ApiClients.Abstractions.ChatApiClient.Conversation;
using ApiClients.Abstractions.ChatApiClient.Conversation.Requests;
using ApiClients.Abstractions.Models.Responses;
using Services.Abstractions.ChatServices;

namespace Services.ChatServices;

internal class ChatConversationService : IChatConversationService
{
    private readonly IChatConversationApiClient chatConversationApiClient;

    public ChatConversationService(IChatConversationApiClient chatConversationApiClient)
    {
        this.chatConversationApiClient = chatConversationApiClient;
    }

    public async Task ChangeNameAsync(Guid id, string newName, CancellationToken cancellationToken = default)
    {
        var response = await chatConversationApiClient.ChangeNameAsync(id, newName, cancellationToken);
        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }

    public async Task<ConversationInfoData> CreateConversationAsync(Guid targetUserId, CancellationToken cancellationToken = default)
    {
        var response = await chatConversationApiClient.CreateConversationAsync(targetUserId, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

        return response.ResponseData;
    }

    public async Task<ConversationInfoData> CreateGroupConversationAsync(string name, IEnumerable<ConversationMemberRequest> memberRequests, CancellationToken cancellationToken = default)
    {
        var response = await chatConversationApiClient.CreateGroupConversationAsync(name, memberRequests, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

        return response.ResponseData;
    }

    public async Task<ConversationInfoData?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationApiClient.FindByIdAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return response.ResponseData;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null; // Conversation not found
        }
    }

    public async Task<MessageData[]> GetAllConversationMessagesAsync(Guid id, DateTimeOffset before, CancellationToken cancellationToken = default)
    {
        var response = await chatConversationApiClient.GetMessagesBefore(id, before, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

        return response.ResponseData;
    }

    public async Task<MessageData[]> GetConversationMessagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await chatConversationApiClient.GetConversationMessagesAsync(id, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

        return response.ResponseData;
    }

    public async Task<ParticipantData[]> GetConversationParticipantsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await chatConversationApiClient.GetConversationParticipantsAsync(id, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

        return response.ResponseData;
    }

    public async Task<MessageData?> GetLatestMessage(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await chatConversationApiClient.GetLatestMessage(id, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

        return response.ResponseData;
    }

    public async Task<MessageData[]> GetUnreadMessagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await chatConversationApiClient.GetUnreadMessagesAsync(id, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

        return response.ResponseData;
    }

    public async Task<ConversationDetailsData[]> GetUserConversationsAsync(CancellationToken cancellationToken = default)
    {
        var response = await chatConversationApiClient.GetUserConversationsAsync(cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

        return response.ResponseData;
    }

    public async Task MarkMessagesAsReadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await chatConversationApiClient.MarkMessagesAsReadAsync(id, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }
}
