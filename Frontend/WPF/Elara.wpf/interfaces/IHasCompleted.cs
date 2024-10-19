using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elara.wpf.interfaces
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
