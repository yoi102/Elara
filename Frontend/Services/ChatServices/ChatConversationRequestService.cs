using ApiClients.Abstractions.ChatApiClient.ConversationRequest;
using ApiClients.Abstractions.Models.Responses;
using Frontend.Shared.Exceptions;
using Services.Abstractions;
using Services.Abstractions.ChatServices;

namespace Services.ChatServices;

internal class ChatConversationRequestService : IChatConversationRequestService
{
    private readonly IChatConversationRequestApiClient chatConversationRequestApiClient;

    public ChatConversationRequestService(IChatConversationRequestApiClient chatConversationRequestApiClient)
    {
        this.chatConversationRequestApiClient = chatConversationRequestApiClient;
    }

    public async Task<ApiServiceResult> AcceptConversationRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationRequestApiClient.AcceptConversationRequestAsync(id, cancellationToken);

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
            else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ApiServiceResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = "Conversation request not found.",
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult<ConversationRequestData>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationRequestApiClient.FindByIdAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return new ApiServiceResult<ConversationRequestData>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<ConversationRequestData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ApiServiceResult<ConversationRequestData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = "Conversation request not found.",
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult<ConversationRequestData[]>> GetConversationRequestsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationRequestApiClient.GetConversationRequestsAsync(cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return new ApiServiceResult<ConversationRequestData[]>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<ConversationRequestData[]>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult> RejectConversationRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationRequestApiClient.RejectConversationRequestAsync(id, cancellationToken);

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
            else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ApiServiceResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = "Conversation request not found.",
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult> SendConversationRequestAsync(Guid receiverId, Guid conversationId, string role, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationRequestApiClient.SendConversationRequestAsync(receiverId, conversationId, role, cancellationToken);

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
