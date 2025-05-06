using ApiClients.Abstractions.ChatApiClient.Message;
using ApiClients.Abstractions.ChatApiClient.Message.Requests;
using Frontend.Shared.Exceptions;
using Services.Abstractions.ChatServices;
using Services.Abstractions.Results;
using Services.Abstractions.Results.Results;

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

    public async Task<MessageResult> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatMessageApiClient.FindByIdAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
            var data = new MessageResultData(
                response.ResponseData.MessageId,
                response.ResponseData.ConversationId,
                response.ResponseData.QuoteMessageId,
                response.ResponseData.Content,
                response.ResponseData.SenderId,
                response.ResponseData.UploadedItemIds,
                response.ResponseData.CreatedAt,
                response.ResponseData.UpdatedAt);
            return new MessageResult()
            {
                IsSuccessful = true,
                ResultData = data
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new MessageResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new MessageResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = "Not Found",
                    IsServerError = true
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<MessagesResult> GetReplyMessagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatMessageApiClient.GetReplyMessagesAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            var data = response.ResponseData.Select(x =>
                                                                    new MessageResultData(
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
