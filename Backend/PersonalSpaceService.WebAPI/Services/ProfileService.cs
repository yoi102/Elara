using DomainCommons.EntityStronglyIds;
using Grpc.Core;
using PersonalSpaceService.Domain.Interfaces;
using Profile;

namespace PersonalSpaceService.WebAPI.Services;

public class ProfileServiceImplementation : ProfileService.ProfileServiceBase
{
    private readonly IPersonalSpaceRepository personalSpaceRepository;

    public ProfileServiceImplementation(IPersonalSpaceRepository personalSpaceRepository)
    {
        this.personalSpaceRepository = personalSpaceRepository;
    }

    public override async Task<GetUserProfileReply> GetUserProfile(GetUserProfileRequest request, ServerCallContext context)
    {
        if (!UserId.TryParse(request.UserId, out var userId))
        {
            return new GetUserProfileReply();
        }

        var profile = await personalSpaceRepository.FindProfileByUserIdAsync(userId);

        if (profile is null)
        {
            return new GetUserProfileReply();
        }

        return new GetUserProfileReply()
        {
            AvatarId = profile.AvatarItemId?.ToString() ?? string.Empty,
            DisplayName = profile.DisplayName,
            UserId = profile.UserId.ToString()
        };
    }
}
