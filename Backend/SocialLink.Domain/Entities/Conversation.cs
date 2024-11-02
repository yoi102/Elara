using DomainCommons;
using Strongly;

namespace SocialLink.Domain.Entities
{
    [Strongly(converters: StronglyConverter.EfValueConverter |
                      StronglyConverter.SwaggerSchemaFilter |
                      StronglyConverter.SystemTextJson |
                      StronglyConverter.TypeConverter)]
    public partial struct ConversationId;

    public class Conversation : Entity<ConversationId>, IHasCreationTime
    {
        public Conversation(string name)
        {
            Id = ConversationId.New();
            Name = name;
            CreationTime = DateTimeOffset.Now;
        }

        private Conversation()
        {
        }

        public DateTimeOffset CreationTime { get; private set; }
        public override ConversationId Id { get; protected set; }
        public ICollection<MessageId> MessageIds { get; set; } = new HashSet<MessageId>();
        public string Name { get; set; } = null!;
        public ICollection<ParticipantId> ParticipantIds { get; set; } = new HashSet<ParticipantId>();
    }
}