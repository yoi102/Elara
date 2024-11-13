using SocialLink.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLink.Domain.Interfaces
{
    public interface IMessageRepository
    {
        Task<Message?> FindByIdAsync(MessageId id);

        Task<Message[]> SearchMessageAsync(string keyword);
    }
}