namespace ApiClients.Abstractions.UserIdentityApiClient.Responses;

public record ResetCodeResponse : ApiResponse<ResetCodeData>;

public record ResetCodeData(string ResetCode);//本不应该出现在Response中
