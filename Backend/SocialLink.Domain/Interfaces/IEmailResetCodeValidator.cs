using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLink.Domain.Interfaces
{
    public interface IEmailResetCodeValidator
    {
        bool Validate(string email, string ResetCode);
        void StashEmailResetCode(string email, string ResetCode);
    }
}
