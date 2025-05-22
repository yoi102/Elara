using DomainCommons.EntityStronglyIds;
using Grpc.Core;
using Identity;
using IdentityService.Infrastructure;

namespace IdentityService.WebAPI.Services;

public class IdentifierServiceImplementation : Identifier.IdentifierBase
{
    private readonly ILogger<IdentifierServiceImplementation> logger;
    private readonly UserRepository userRepository;

    public IdentifierServiceImplementation(ILogger<IdentifierServiceImplementation> logger, UserRepository userRepository)
    {
        this.logger = logger;
        this.userRepository = userRepository;
    }

    public override async Task<AccountInfoReply> GetAccountInfo(AccountInfoRequest request, ServerCallContext context)
    {
        if (!UserId.TryParse(request.Id, out var userId))
        {
            return new AccountInfoReply();
        }

        var user = await userRepository.FindByIdAsync(userId);

        if (user == null) return new AccountInfoReply();

        return new AccountInfoReply
        {
            Id = user.Id.ToString(),
            Name = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            CreatedAt = user.CreatedAt.ToString()
        };
    }
}
