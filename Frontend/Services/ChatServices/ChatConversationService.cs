using ApiClients.Abstractions.ChatApiClient.Conversation;
using ApiClients.Abstractions.ChatApiClient.Conversation.Requests;
using ApiClients.Abstractions.Models.Responses;
using Frontend.Shared.Exceptions;
using Services.Abstractions;
using Services.Abstractions.ChatServices;

namespace Services.ChatServices;

internal class ChatConversationService : IChatConversationService
{
    private readonly IChatConversationApiClient chatConversationApiClient;

    public ChatConversationService(IChatConversationApiClient chatConversationApiClient)
    {
        this.chatConversationApiClient = chatConversationApiClient;
    }

    public async Task<ApiServiceResult> ChangeNameAsync(Guid id, string newName, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationApiClient.ChangeNameAsync(id, newName, cancellationToken);
            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
            return new ApiServiceResult()
            {
                IsSuccessful = true
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                return new ApiServiceResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = "The name is already in use."
                };
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ApiServiceResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = "Conversation not found.",
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult<ConversationInfoData>> CreateConversationAsync(Guid targetUserId, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationApiClient.CreateConversationAsync(targetUserId, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return new ApiServiceResult<ConversationInfoData>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData,
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<ConversationInfoData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult<ConversationInfoData>> CreateGroupConversationAsync(string name, IEnumerable<ConversationMemberRequest> memberRequests, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationApiClient.CreateGroupConversationAsync(name, memberRequests, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return new ApiServiceResult<ConversationInfoData>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<ConversationInfoData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                return new ApiServiceResult<ConversationInfoData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = "The name is already in use."
                };
            }
            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult<ConversationInfoData>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationApiClient.FindByIdAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return new ApiServiceResult<ConversationInfoData>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<ConversationInfoData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ApiServiceResult<ConversationInfoData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = "Conversation not found.",
                };
            }
            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult<MessageData[]>> GetAllConversationMessagesAsync(Guid id, DateTimeOffset before, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationApiClient.GetMessagesBefore(id, before, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return new ApiServiceResult<MessageData[]>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<MessageData[]>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult<MessageData[]>> GetConversationMessagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationApiClient.GetConversationMessagesAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return new ApiServiceResult<MessageData[]>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<MessageData[]>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult<ParticipantData[]>> GetConversationParticipantsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationApiClient.GetConversationParticipantsAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return new ApiServiceResult<ParticipantData[]>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<ParticipantData[]>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceSimpleResult<MessageData>> GetLatestMessage(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationApiClient.GetLatestMessage(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return new ApiServiceSimpleResult<MessageData>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceSimpleResult<MessageData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult<MessageData[]>> GetUnreadMessagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationApiClient.GetUnreadMessagesAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return new ApiServiceResult<MessageData[]>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<MessageData[]>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult<ConversationDetailsData[]>> GetUserConversationsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationApiClient.GetUserConversationsAsync(cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return new ApiServiceResult<ConversationDetailsData[]>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<ConversationDetailsData[]>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }

            throw new ApiResponseException();
        }
    }
    public async Task<ApiServiceResult> MarkMessagesAsReadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationApiClient.MarkMessagesAsReadAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
            return new ApiServiceResult()
            {
                IsSuccessful = true
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                return new ApiServiceResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = "The user is already a participant in the conversation.",
                };
            }

            throw new ApiResponseException();
        }
    }
}
