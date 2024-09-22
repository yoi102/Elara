using DomainCommons.EntityStronglyIds;
using Grpc.Core;
using Identity;
using IdentityService.Infrastructure;

namespace IdentityService.WebAPI.Services;

public class IdentifierService : Identifier.IdentifierBase
{
    private readonly ILogger<IdentifierService> logger;
    private readonly UserRepository userRepository;

    public IdentifierService(ILogger<IdentifierService> logger, UserRepository userRepository)
    {
        this.logger = logger;
        this.userRepository = userRepository;
    }

    public override async Task<UserInfoReply> GetUserInfo(UserInfoRequest request, ServerCallContext context)
    {
        if (!UserId.TryParse(request.Id, out var userId))
        {
            return new UserInfoReply();
        }

        var user = await userRepository.FindByIdAsync(userId);

        if (user == null) return new UserInfoReply();

        return new UserInfoReply { UserName = user.UserName, Email = user.Email, PhoneNumber = user.PhoneNumber };
    }
}
