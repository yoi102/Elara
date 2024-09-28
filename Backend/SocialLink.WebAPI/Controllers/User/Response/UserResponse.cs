using SocialLink.Domain.Entities;

namespace SocialLink.WebAPI.Controllers.User.Response
{
    public record UserResponse(UserId Id, string Name, DateTimeOffset CreationTime);
}
