using SocialLink.Domain.Entities;

namespace SocialLink.WebAPI.Controllers.User.Response
{
    public record GetUserInfoResponse(UserId Id, string Name, string? Email, string? PhoneNumber, DateTimeOffset CreationTime);
}
