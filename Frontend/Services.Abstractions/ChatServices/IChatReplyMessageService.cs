using ApiClients.Abstractions.ChatApiClient.ReplyMessage.Requests;
using Services.Abstractions.Results;

namespace Services.Abstractions.ChatServices;

public interface IChatReplyMessageService
{
    Task<ApiServiceResult> ReplyMessageAsync(ReplyMessageRequest replyMessageRequest, CancellationToken cancellationToken = default);
}
