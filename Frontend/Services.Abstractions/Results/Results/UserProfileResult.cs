namespace Services.Abstractions.Results.Results;

public record UserProfileResult : ApiServiceResult<UserProfileResultData>;

public record UserProfileResultData(Guid Id, string DisplayName, Guid AvatarItemId, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt);
