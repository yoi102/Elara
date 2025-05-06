using ApiClients.Abstractions.ChatApiClient.ReplyMessage.Requests;
using ApiClients.Abstractions.ChatApiClient.ReplyMessage.Responses;

namespace ApiClients.Abstractions.ChatApiClient.ReplyMessage;

public interface IChatReplyMessageApiClient
{
    Task<ReplyMessageResponse> ReplyMessageAsync(ReplyMessageRequest replyMessageRequest, CancellationToken cancellationToken = default);
}
