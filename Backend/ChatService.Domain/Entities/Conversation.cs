using DomainCommons.EntityStronglyIds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Domain.Entities
{
    public class Conversation
    {


        public UserId User1Id { get; set; }
        public UserId User2Id { get; set; }

        public List<MessageId> MessageIds { get; set; } = new List<MessageId>();




    }
}
