using DomainCommons.EntityStronglyIds;

namespace PersonalSpaceService.WebAPI.Controllers.ProfileController.Results;
public record ProfileResult
{
    public required UserId UserId { get; init; }

    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Name { get; init; }
    public required string DisplayName { get; init; }
    public required string? AvatarUrl { get; init; }
}
