using ApiClients.Abstractions.ChatApiClient.Message.Requests;

namespace Services.Abstractions.ChatServices;

public interface IChatReplyMessageService
{
    Task<ApiServiceResult> ReplyMessageAsync(ReplyMessageRequest replyMessageRequest, CancellationToken cancellationToken = default);
}
