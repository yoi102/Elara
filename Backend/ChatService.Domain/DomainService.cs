using ChatService.Domain.Entities;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain;

public class DomainService
{
    private readonly IChatServiceRepository chatServiceRepository;

    public DomainService(IChatServiceRepository chatServiceRepository)
    {
        this.chatServiceRepository = chatServiceRepository;
    }

    public async Task<GroupConversation?> ChangeGroupConversationNameAsync(GroupConversationId groupConversationId, string name)
    {
        var groupConversation = await chatServiceRepository.FindGroupConversationByIdAsync(groupConversationId);
        groupConversation?.ChangeName(name);

        return groupConversation;
    }

    public async Task<GroupMessage?> UpdateGroupMessageAsync(MessageId messageId, string content, Uri[] attachments)
    {
        var message = await chatServiceRepository.FindGroupMessageByIdAsync(messageId);

        message?.ChangeContent(content);
        message?.ChangeAttachments(attachments);
        return message;
    }

    public async Task<PersonalMessage?> UpdatePersonalMessageAsync(MessageId messageId, string content, Uri[] attachments)
    {
        var message = await chatServiceRepository.FindPersonalMessageByIdAsync(messageId);

        message?.ChangeContent(content);
        message?.ChangeAttachments(attachments);
        return message;
    }

    public async Task<ReplyMessage?> UpdateReplyMessageAsync(MessageId messageId, string content, Uri[] attachments)
    {
        var message = await chatServiceRepository.FindReplyMessageByIdAsync(messageId);

        message?.ChangeContent(content);
        message?.ChangeAttachments(attachments);
        return message;
    }
}
