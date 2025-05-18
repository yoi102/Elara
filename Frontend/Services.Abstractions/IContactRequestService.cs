using Services.Abstractions.Results;
using Services.Abstractions.Results.Data;

namespace Services.Abstractions;

public interface IContactRequestService
{
    Task<ApiServiceResult<ContactRequestData[]>> GetUserContactRequests();

    Task<ApiServiceResult<ContactRequestData>> GetUserContactRequest(Guid contactRequestId);
}
