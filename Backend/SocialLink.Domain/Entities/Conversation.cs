using DomainCommons;
using Strongly;

namespace SocialLink.Domain.Entities
{
    [Strongly(converters: StronglyConverter.EfValueConverter |
                      StronglyConverter.SwaggerSchemaFilter |
                      StronglyConverter.SystemTextJson |
                      StronglyConverter.TypeConverter)]
    public partial struct ConversationId;

    public class Conversation : AggregateRootEntity<ConversationId>
    {
        public Conversation(string name)
        {
            Id = ConversationId.New();
            Name = name;
        }

        private Conversation()
        {
        }


        public override ConversationId Id { get; protected set; }
        public ICollection<MessageId> MessageIds { get;private set; } = new HashSet<MessageId>();
        public string Name { get; set; } = null!;
        public ICollection<ParticipantId> ParticipantIds { get; private set; } = new HashSet<ParticipantId>();
    }
}