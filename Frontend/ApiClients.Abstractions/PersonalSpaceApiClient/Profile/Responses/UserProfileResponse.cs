namespace ApiClients.Abstractions.PersonalSpaceApiClient.Profile.Responses;

public record UserProfileResponse : ApiResponse<UserProfileData>;

public record UserProfileData(Guid Id, string DisplayName, Guid AvatarItemId);
