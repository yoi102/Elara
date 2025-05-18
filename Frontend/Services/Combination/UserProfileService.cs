using ApiClients.Abstractions.FileApiClient.Responses;
using Services.Abstractions;
using Services.Abstractions.PersonalSpaceServices;
using Services.Abstractions.Results;
using Services.Abstractions.Results.Data;

namespace Services.Combination;

public class UserProfileService : IUserProfileService
{
    private readonly IPersonalSpaceProfileService profileService;
    private readonly IFileService fileService;

    public UserProfileService(IPersonalSpaceProfileService profileService, IFileService fileService)
    {
        this.profileService = profileService;
        this.fileService = fileService;
    }

    public async Task<ApiServiceResult<UserInfoData>> GetUserInfoDataById(Guid userId, CancellationToken cancellationToken = default)
    {
        var profileResult = await profileService.GetUserProfileAsync(userId, cancellationToken);
        if (!profileResult.IsSuccessful)
            return ApiServiceResult<UserInfoData>.FromFailure(profileResult);

        FileItemData? avatarItem = null;
        if (profileResult.ResultData.AvatarItemId is not null)
        {
            var avatarItemResult = await fileService.GetFileItemAsync(profileResult.ResultData.AvatarItemId.Value, cancellationToken);
            if (!avatarItemResult.IsSuccessful)
                return ApiServiceResult<UserInfoData>.FromFailure(profileResult);

            avatarItem = avatarItemResult.ResultData;
        }

        var sender = new UserInfoData()
        {
            UserId = profileResult.ResultData.UserId,
            DisplayName = profileResult.ResultData.DisplayName,
            Avatar = avatarItem,
            CreatedAt = profileResult.ResultData.CreatedAt,
            UpdatedAt = profileResult.ResultData.UpdatedAt,
        };

        return new ApiServiceResult<UserInfoData>()
        {
            IsSuccessful = true,
            IsServerError = false,
            ResultData = sender,
        };
    }
}
