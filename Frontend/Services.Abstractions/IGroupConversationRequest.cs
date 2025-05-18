using Services.Abstractions.Results;
using Services.Abstractions.Results.Data;

namespace Services.Abstractions;

public interface IGroupConversationRequest
{
    Task<ApiServiceResult<GroupConversationRequestData[]>> GetCurrentUserConversationRequestsAsync();

    Task<ApiServiceResult<GroupConversationRequestData>> GetCurrentUserConversationRequestsAsync(Guid id);
}
