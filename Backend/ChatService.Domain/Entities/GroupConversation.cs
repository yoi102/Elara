using ChatService.Domain.Events;
using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities {

    public record GroupConversation : AggregateRootEntity<GroupConversationId> {
        private readonly List<UserId> member = [];

        public GroupConversation(string name)
        {
            Name = name;
            Id = GroupConversationId.New();
        }
        private GroupConversation()
        {
            
        }

        public override GroupConversationId Id { get; protected set; }
        public IReadOnlyCollection<UserId> Member => member.AsReadOnly();
        public string Name { get; set; } = string.Empty;

        public void AddMember(UserId userId)
        {
            member.Add(userId);
            AddDomainEvent(new GroupConversationUpdatedEvent(this));
        }

        public void AddMember(UserId[] userIds)
        {
            member.AddRange(userIds);
            AddDomainEvent(new GroupConversationUpdatedEvent(this));
        }

        public void ChangeName(string name)
        {
            Name = name;
            AddDomainEvent(new GroupConversationUpdatedEvent(this));
        }

        public void RemoveMember(UserId userId)
        {
            member.Remove(userId);
            AddDomainEvent(new GroupConversationUpdatedEvent(this));
        }
    }
}
