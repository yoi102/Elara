namespace IdentityService.Domain.Interfaces
{
    public interface IEmailResetCodeValidator
    {
        bool Validate(string email, string ResetCode);

        void StashEmailResetCode(string email, string ResetCode);
    }
}