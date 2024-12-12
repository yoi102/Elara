using DomainCommons.EntityStronglyIds;
using Grpc.Core;
using IdentityService.Infrastructure;
using IdentityService.WebAPI.Protos;

namespace IdentityService.WebAPI.Services
{
    public class IdentityService : Identity.IdentityBase
    {
        private readonly ILogger<IdentityService> logger;
        private readonly UserRepository userRepository;

        public IdentityService(ILogger<IdentityService> logger, UserRepository userRepository)
        {
            this.logger = logger;
            this.userRepository = userRepository;
        }

        public override async Task<UserInfoReply> GetUserInfo(UserInfoRequest request, ServerCallContext context)
        {
            var stringId = request.Id;
            if (!UserId.TryParse(request.Id, out var userId))
            {
                return new UserInfoReply();
            }
            
            var user = await userRepository.FindByIdAsync(userId);

            if (user == null) return new UserInfoReply();

            return new UserInfoReply { UserName = user.UserName };
        }
    }
}