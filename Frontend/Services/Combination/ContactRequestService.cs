using Services.Abstractions;
using Services.Abstractions.PersonalSpaceServices;
using Services.Abstractions.Results;
using Services.Abstractions.Results.Data;

namespace Services.Combination;

public class ContactRequestService : IContactRequestService
{
    private readonly IPersonalSpaceContactRequestService contactRequestService;
    private readonly IUserProfileService userProfileService;

    public ContactRequestService(IPersonalSpaceContactRequestService contactRequestService, IUserProfileService userProfileService)
    {
        this.contactRequestService = contactRequestService;
        this.userProfileService = userProfileService;
    }

    public async Task<ApiServiceResult<ContactRequestData[]>> GetUserContactRequests()
    {
        var contactsDataResult = await contactRequestService.GetReceivedContactRequestsAsync();
        if (!contactsDataResult.IsSuccessful)
            return ApiServiceResult<ContactRequestData[]>.FromFailure(contactsDataResult);
        var contacts = new List<ContactRequestData>();
        foreach (var data in contactsDataResult.ResultData)
        {
            var senderInfoResult = await userProfileService.GetUserInfoDataById(data.SenderId);
            if (!senderInfoResult.IsSuccessful)
                return ApiServiceResult<ContactRequestData[]>.FromFailure(senderInfoResult);

            var contact = new ContactRequestData()
            {
                Id = data.Id,
                Sender = senderInfoResult.ResultData,
                Status = data.Status,
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
            };
            contacts.Add(contact);
        }

        return new ApiServiceResult<ContactRequestData[]>()
        {
            IsSuccessful = true,
            IsServerError = false,
            ResultData = contacts.ToArray(),
        };
    }

    public async Task<ApiServiceResult<ContactRequestData>> GetUserContactRequest(Guid contactRequestId)
    {
        var contactsDataResult = await contactRequestService.GetContactRequestByIdAsync(contactRequestId);
        if (!contactsDataResult.IsSuccessful)
            return ApiServiceResult<ContactRequestData>.FromFailure(contactsDataResult);
        var data = contactsDataResult.ResultData;

        var senderInfoResult = await userProfileService.GetUserInfoDataById(data.SenderId);
        if (!senderInfoResult.IsSuccessful)
            return ApiServiceResult<ContactRequestData>.FromFailure(senderInfoResult);

        var contact = new ContactRequestData()
        {
            Id = data.Id,
            Sender = senderInfoResult.ResultData,
            Status = data.Status,
            CreatedAt = data.CreatedAt,
            UpdatedAt = data.UpdatedAt,
        };

        return new ApiServiceResult<ContactRequestData>()
        {
            IsSuccessful = true,
            IsServerError = false,
            ResultData = contact,
        };
    }
}
