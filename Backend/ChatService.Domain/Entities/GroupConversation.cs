using DomainCommons.EntityStronglyIds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Domain.Entities
{
    public class GroupConversation
    {
        public GroupConversation(string name)
        {
            Name = name;
                
        }
        public string Name { get; set; } 


        public List<MessageId> MessageIds { get; set; } = new List<MessageId>();

    }
}
