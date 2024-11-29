using IdentityService.Domain.Entities;

namespace IdentityService.WebAPI.Controllers.User.Response
{
    public record GetUserInfoResponse(UserId Id, string Name, string? Email, string? PhoneNumber, DateTimeOffset CreationTime);
}