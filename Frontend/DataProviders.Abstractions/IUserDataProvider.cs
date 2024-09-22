namespace DataProviders.Abstractions;

public interface IUserDataProvider
{
    Guid UserId { get; }
    string? UserName { get; }
    string? RefreshToken { get; }
    string? AccessToken { get; }

    void UpdateUser(Guid userId, string? userName, string refreshToken);

    void UpdateRefreshToken(string accessToken, string refreshToken);
}
