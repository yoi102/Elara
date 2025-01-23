namespace Service.Abstractions.UserResponses;

public record GetUserInfoResponse() : ResponseBase
{
    public UserInfo? UserInfo { get; set; }
};