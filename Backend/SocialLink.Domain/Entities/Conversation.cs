using DomainCommons;
using Strongly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLink.Domain.Entities
{
    [Strongly(converters: StronglyConverter.EfValueConverter |
                      StronglyConverter.SwaggerSchemaFilter |
                      StronglyConverter.SystemTextJson |
                      StronglyConverter.TypeConverter)]
    public partial struct ConversationId;
    public class Conversation : Entity<ConversationId>
    {
        public override ConversationId Id => throw new NotImplementedException();

        //public string Id { get; set; }   // 对话唯一标识
        //public string Name { get; set; } // 群聊名字（如果是群聊）
        //public List<User> Participants { get; set; } // 参与者
        //public DateTime CreatedAt { get; set; }      // 创建时间
        //public Message LastMessage { get; set; }     // 最近的消息

    }
}
