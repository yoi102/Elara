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
    public partial struct WorkspaceConversationId;

    public class WorkspaceConversation : AggregateRootEntity<PersonalConversationId>
    {
        public WorkspaceConversation(string name)
        {
            Id = PersonalConversationId.New();
            Name = name;
        }

        private WorkspaceConversation()
        {
        }


        public override PersonalConversationId Id { get; protected set; }
        public ICollection<MessageId> MessageIds { get; private set; } = new HashSet<MessageId>();
        public string Name { get; set; } = null!;
        public ICollection<ParticipantId> ParticipantIds { get; private set; } = new HashSet<ParticipantId>();
    }


}
