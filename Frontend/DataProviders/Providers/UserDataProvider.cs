using DataProviders.Abstractions;

namespace DataProviders.Providers;

internal class UserDataProvider : IUserDataProvider
{
    public Guid UserId { get; private set; }
    public string? UserName { get; private set; }
    public string? RefreshToken { get; private set; }
    public string? AccessToken { get; private set; }

    public void UpdateRefreshToken(string accessToken, string refreshToken)
    {
        RefreshToken = refreshToken;
        AccessToken = accessToken;
    }

    public void UpdateUser(Guid userId, string? userName, string refreshToken)
    {
        UserId = userId;
        UserName = userName;
        RefreshToken = refreshToken;
    }
}
