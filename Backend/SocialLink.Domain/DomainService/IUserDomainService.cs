using Microsoft.AspNetCore.Identity;
using SocialLink.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLink.Domain.DomainService
{
    public interface IUserDomainService
    {
        Task<LoginResult> LoginByEmailAndPasswordAsync(string email, string password);
        Task<LoginResult> LoginByNameAndPasswordAsync(string name, string password);
        Task<LoginResult> LoginByPhoneNumberAndPasswordAsync(string phoneNumber, string password);
    }
}
