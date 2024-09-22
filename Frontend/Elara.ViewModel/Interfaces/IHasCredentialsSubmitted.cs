namespace Elara.ViewModel.Interfaces;

public interface IHasCredentialsSubmitted
{
    event EventHandler<AccountCredentialsEventArgs>? CredentialsSubmitted;
}

public class AccountCredentialsEventArgs : EventArgs
{
    public required string NameOrEmail { get; set; }
    public required string Password { get; set; }
}
