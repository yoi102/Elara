using DomainCommons.EntityStronglyIds;
using Grpc.Core;
using PersonalSpaceService.Domain.Interfaces;
using Profile;

namespace PersonalSpaceService.WebAPI.Services;

public class ProfilerService : Profiler.ProfilerBase
{
    private readonly IPersonalSpaceRepository repository;

    public ProfilerService(ILogger<ProfilerService> logger, IPersonalSpaceRepository repository)
    {
        this.repository = repository;
    }

    public override async Task<ProfileReply> GetProfileInfo(ProfileRequest request, ServerCallContext context)
    {
        ProfileReply reply = new ProfileReply();

        if (!UserId.TryParse(request.UserId, out var userId))
        {
            return reply;
        }

        var profile = await repository.FindProfileByUserIdAsync(userId);

        if (profile == null)
        {
            return reply;
        }

        reply.DisplayName = profile.DisplayName;
        reply.AvatarId = profile.AvatarItemId.ToString();
        return reply;
    }
}
