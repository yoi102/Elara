using ApiClients.Abstractions.ChatApiClient.ReplyMessage;
using ApiClients.Abstractions.ChatApiClient.ReplyMessage.Requests;
using Frontend.Shared.Exceptions;
using Services.Abstractions.ChatServices;
using Services.Abstractions.Results;

namespace Services.ChatServices;

internal class ChatReplyMessageService : IChatReplyMessageService
{
    private readonly IChatReplyMessageApiClient chatReplyMessageApiClient;

    public ChatReplyMessageService(IChatReplyMessageApiClient chatReplyMessageApiClient)
    {
        this.chatReplyMessageApiClient = chatReplyMessageApiClient;
    }

    public async Task<ApiServiceResult> ReplyMessageAsync(ReplyMessageRequest replyMessageRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatReplyMessageApiClient.ReplyMessageAsync(replyMessageRequest, cancellationToken);

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
}
