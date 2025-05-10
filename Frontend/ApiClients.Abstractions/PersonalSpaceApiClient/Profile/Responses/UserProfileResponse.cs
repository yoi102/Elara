namespace ApiClients.Abstractions.PersonalSpaceApiClient.Profile.Responses;

public record UserProfileResponse : ApiResponse<UserProfileData>;

public record UserProfileData(Guid Id,Guid UserId, string DisplayName, Guid AvatarItemId, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt);
