namespace IdentityService.WebAPI.Events.Args
{
    public record class ResetPasswordByEmailResetCodeEventArgs(string Email, string Subject, string HtmlMessage);


}
