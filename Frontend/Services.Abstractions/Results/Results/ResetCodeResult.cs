namespace Services.Abstractions.Results.Results;

public record ResetCodeResult : ApiServiceResult<ResetCodeResultData>;

public record ResetCodeResultData(string ResetCode);
