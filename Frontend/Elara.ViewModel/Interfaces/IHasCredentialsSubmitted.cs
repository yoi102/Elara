namespace Elara.ViewModel.Interfaces;

public interface IHasCredentialsSubmitted
{
    event EventHandler<AccountCredentialsEventArgs>? CredentialsSubmitted;
}

public class AccountCredentialsEventArgs : EventArgs
{
    public string NameOrEmail { get; set; }
    public string Password { get; set; }
}
