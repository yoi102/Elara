using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain.Results;

public record class GetEmailResetCodeResult(IdentityResult IdentityResult, string Email, string Subject, string HtmlMessage);