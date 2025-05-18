using Services.Abstractions.Results;
using Services.Abstractions.Results.Data;

namespace Services.Abstractions;
public interface IContactService
{
    Task<ApiServiceResult<ContactInfoData[]>> GetContactsAsync();
}
