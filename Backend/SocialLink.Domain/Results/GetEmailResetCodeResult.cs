using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLink.Domain.Results
{
    public record class GetEmailResetCodeResult(IdentityResult IdentityResult,string Email, string Subject, string HtmlMessage);
}
