using ApiClients.Abstractions.ChatApiClient.Message;
using ApiClients.Abstractions.ChatApiClient.Message.Requests;
using ApiClients.Abstractions.Models.Responses;
using Frontend.Shared.Exceptions;
using Services.Abstractions;
using Services.Abstractions.ChatServices;

namespace Services.ChatServices;

internal class ChatMessageService : IChatMessageService
{
    private readonly IChatMessageApiClient chatMessageApiClient;

    public ChatMessageService(IChatMessageApiClient chatMessageApiClient)
    {
        this.chatMessageApiClient = chatMessageApiClient;
    }

    public async Task<ApiServiceResult> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatMessageApiClient.DeleteByIdAsync(id, cancellationToken);

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

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult<MessageWithReplyMessageData>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatMessageApiClient.FindByIdAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return new ApiServiceResult<MessageWithReplyMessageData>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<MessageWithReplyMessageData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ApiServiceResult<MessageWithReplyMessageData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = "Not Found",
                    IsServerError = true
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult<MessageWithReplyMessageData[]>> GetBatch(Guid[] ids, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatMessageApiClient.GetBatch(ids, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return new ApiServiceResult<MessageWithReplyMessageData[]>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<MessageWithReplyMessageData[]>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult<ReplyMessageData[]>> GetReplyMessagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatMessageApiClient.GetReplyMessagesAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return new ApiServiceResult<ReplyMessageData[]>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<ReplyMessageData[]>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult> ReplyMessageAsync(ReplyMessageRequest replyMessageRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatMessageApiClient.ReplyMessageAsync(replyMessageRequest, cancellationToken);

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
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult> SendMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatMessageApiClient.SendMessageAsync(messageData, cancellationToken);

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

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult> UpdateMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatMessageApiClient.UpdateMessageAsync(messageData, cancellationToken);

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
                    ErrorMessage = "Not Found",
                    IsServerError = true
                };
            }
            throw new ApiResponseException();
        }
    }
}
