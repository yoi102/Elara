using Services.Abstractions;
using Services.Abstractions.PersonalSpaceServices;
using Services.Abstractions.Results;
using Services.Abstractions.Results.Data;

namespace Services.Combination;

public class ContactService : IContactService
{
    private readonly IPersonalSpaceContactService personalSpaceContactService;
    private readonly IUserProfileService userProfileService;

    public ContactService(IPersonalSpaceContactService personalSpaceContactService, IUserProfileService userProfileService)
    {
        this.personalSpaceContactService = personalSpaceContactService;
        this.userProfileService = userProfileService;
    }

    public async Task<ApiServiceResult<ContactInfoData[]>> GetContactsAsync()
    {
        var apiServiceResult = await personalSpaceContactService.GetContactsAsync();
        if (!apiServiceResult.IsSuccessful)
            return ApiServiceResult<ContactInfoData[]>.FromFailure(apiServiceResult);
        var contacts = new List<ContactInfoData>();
        foreach (var data in apiServiceResult.ResultData)
        {
            var userInfoResult = await userProfileService.GetUserInfoDataById(data.Id);
            if (!userInfoResult.IsSuccessful)
                return ApiServiceResult<ContactInfoData[]>.FromFailure(userInfoResult);

            var contact = new ContactInfoData()
            {
                Id = data.Id,
                Remark = data.Remark,
                UserInfo = userInfoResult.ResultData,

                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
            };

            contacts.Add(contact);
        }
        return new ApiServiceResult<ContactInfoData[]>()
        {
            IsSuccessful = true,
            IsServerError = false,
            ResultData = contacts.ToArray(),
        };
    }
}
