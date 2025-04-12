using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASPNETCore;

public class AuthorizedUserController : ControllerBase
{
    protected UserId GetCurrentUserId()
    {
        var stringUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(stringUserId))
        {
            throw new UnauthorizedAccessException("User identity is missing.");
        }

        if (!UserId.TryParse(stringUserId, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user ID format.");
        }

        return userId;
    }
}
