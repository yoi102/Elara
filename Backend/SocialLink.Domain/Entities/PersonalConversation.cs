using DomainCommons;
using Strongly;

namespace SocialLink.Domain.Entities
{
    [Strongly(converters: StronglyConverter.EfValueConverter |
                      StronglyConverter.SwaggerSchemaFilter |
                      StronglyConverter.SystemTextJson |
                      StronglyConverter.TypeConverter)]
    public partial struct PersonalConversationId;

    public class PersonalConversation : AggregateRootEntity<PersonalConversationId>
    {
        public PersonalConversation(string name)
        {
            Id = PersonalConversationId.New();
            Name = name;
        }

        private PersonalConversation()
        {
        }


        public override PersonalConversationId Id { get; protected set; }
        public ICollection<MessageId> MessageIds { get;private set; } = new HashSet<MessageId>();
        public string Name { get; set; } = null!;
        public ICollection<ParticipantId> ParticipantIds { get; private set; } = new HashSet<ParticipantId>();
    }
}