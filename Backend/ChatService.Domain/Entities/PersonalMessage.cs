using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities;

public record PersonalMessage : BaseMessage
{
    public PersonalMessage(UserId senderId, PersonalConversationId personalConversationId,
                        string content, Uri[] attachments) : base(senderId, content, attachments)
    {
        this.PersonalConversationId = personalConversationId;
        Id = MessageId.New();
    }
    private PersonalMessage()
    {
    }

    public MessageId? Quote { get; private set; }
    public PersonalConversationId PersonalConversationId { get; private set; }
}
