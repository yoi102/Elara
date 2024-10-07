using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IUserService
    {
        Task<bool> DeleteAsync(CancellationToken cancellationToken = default);
        Task<bool> GetEmailResetCodeAsync(string email, CancellationToken cancellationToken = default);

    }
}
