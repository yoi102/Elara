using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions.UserResponses
{
    public record UserInfo(Guid Id, string Name, string? Email, string? PhoneNumber, DateTimeOffset CreationTime);

}
