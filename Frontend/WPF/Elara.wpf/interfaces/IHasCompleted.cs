namespace Elara.wpf.Interfaces
{
    internal interface IHasCompleted
    {
        event EventHandler<AccountEventArgs>? Completed;
    }

    public class AccountEventArgs : EventArgs
    {
        public required string NameOrEmail { get; set; }
        public required string Password { get; set; }
    }
}
