using ApiClients.Abstractions.ChatApiClient.Conversation;
using ApiClients.Abstractions.ChatApiClient.Conversation.Requests;
using Frontend.Shared.Exceptions;
using Services.Abstractions.ChatServices;
using Services.Abstractions.Results;
using Services.Abstractions.Results.Results;

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

    public async Task<ConversationResult> CreateConversationAsync(Guid targetUserId, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationApiClient.CreateConversationAsync(targetUserId, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            var data = new ConversationResultData(
                response.ResponseData.Id,
                response.ResponseData.Name,
                response.ResponseData.IsGroup,
                response.ResponseData.CreatedAt,
                response.ResponseData.UpdatedAt);

            return new ConversationResult()
            {
                IsSuccessful = true,
                ResultData = data
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ConversationResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            throw new ApiResponseException();
        }
    }

    public async Task<ConversationResult> CreateGroupConversationAsync(string name, IEnumerable<ConversationMemberRequest> memberRequests, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationApiClient.CreateGroupConversationAsync(name, memberRequests, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            var data = new ConversationResultData(
                response.ResponseData.Id,
                response.ResponseData.Name,
                response.ResponseData.IsGroup,
                response.ResponseData.CreatedAt,
                response.ResponseData.UpdatedAt);

            return new ConversationResult()
            {
                IsSuccessful = true,
                ResultData = data
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ConversationResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                return new ConversationResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = "The name is already in use."
                };
            }
            throw new ApiResponseException();
        }
    }

    public async Task<ConversationResult> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationApiClient.FindByIdAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            var data = new ConversationResultData(
                response.ResponseData.Id,
                response.ResponseData.Name,
                response.ResponseData.IsGroup,
                response.ResponseData.CreatedAt,
                response.ResponseData.UpdatedAt);

            return new ConversationResult()
            {
                IsSuccessful = true,
                ResultData = data
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ConversationResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ConversationResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = "Conversation not found.",
                };
            }
            throw new ApiResponseException();
        }
    }

    public async Task<ConversationsResult> GetAllConversationAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationApiClient.GetAllConversationAsync(cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
            var data = response.ResponseData.Select(x => new ConversationResultData(
                   x.Id,
                   x.Name,
                   x.IsGroup,
                   x.CreatedAt,
                   x.UpdatedAt)).ToArray();

            return new ConversationsResult()
            {
                IsSuccessful = true,
                ResultData = data
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ConversationsResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<MessagesResult> GetAllConversationMessagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationApiClient.GetAllConversationMessagesAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
            var data = response.ResponseData.Select(x => new MessageResultData(
                   x.MessageId,
                   x.ConversationId,
                   x.QuoteMessageId,
                   x.Content,
                   x.SenderId,
                   x.UploadedItemIds,
                   x.CreatedAt,
                   x.UpdatedAt)).ToArray();

            return new MessagesResult()
            {
                IsSuccessful = true,
                ResultData = data
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new MessagesResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<UnreadMessagesResult> GetUnreadMessagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationApiClient.GetUnreadMessagesAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
            var data = response.ResponseData.Select(x => new UnreadMessageResultData(
                  x.Id,
                  x.ConversationId,
                  x.UserId,
                  x.MessageId)).ToArray();

            return new UnreadMessagesResult()
            {
                IsSuccessful = true,
                ResultData = data
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new UnreadMessagesResult()
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
