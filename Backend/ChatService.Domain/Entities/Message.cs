using DomainCommons.EntityStronglyIds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Domain.Entities
{
    public class Message
    {





        public UserId SenderId { get; private set; }
        public UserId? ReceiverId { get; private set; }
        public GroupConversationId? GroupId { get; set; }   // 群组聊天的群组 ID（为空表示一对一聊天）

        public string? Content { get; private set; }
        public DateTime SentAt { get; private set; }
        public bool IsRead { get; private set; }





    }







}
