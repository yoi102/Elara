using DomainCommons.EntityStronglyIds;
using PersonalSpaceService.Domain.Entities;
using PersonalSpaceService.Domain.Interfaces;

namespace PersonalSpaceService.Domain;

public class PersonalSpaceDomainService
{
    private readonly IPersonalSpaceRepository personalSpaceRepository;

    public PersonalSpaceDomainService(IPersonalSpaceRepository personalSpaceRepository)
    {
        this.personalSpaceRepository = personalSpaceRepository;
    }

    public async Task<Profile?> UpdateProfileAsync(UserId userId, string displayName, UploadedItemId avatar)
    {
        var profile = await personalSpaceRepository.FindProfileByUserIdAsync(userId);
        if (profile == null)
            return null;
        profile.ChangeAvatar(avatar);
        profile.ChangeDisplayName(displayName);
        return profile;
    }
}
