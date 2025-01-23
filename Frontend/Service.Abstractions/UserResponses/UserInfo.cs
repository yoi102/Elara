namespace Service.Abstractions.UserResponses;

public record UserInfo(Guid Id, string Name, string? Email, string? PhoneNumber, DateTimeOffset CreationTime);
