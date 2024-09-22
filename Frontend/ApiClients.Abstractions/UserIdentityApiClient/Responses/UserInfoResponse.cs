namespace ApiClients.Abstractions.UserIdentityApiClient.Responses;

public record UserInfoResponse : ApiResponse<UserInfo>;

public record UserInfo(Guid Id, string Name, string? Email, string? PhoneNumber, DateTimeOffset CreationTime);
