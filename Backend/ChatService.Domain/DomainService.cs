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

    public async Task<Conversation?> ChangeConversationNameAsync(ConversationId conversationId, string name)
    {
        var conversation = await chatServiceRepository.FindConversationByIdAsync(conversationId);
        conversation?.ChangeName(name);

        return conversation;
    }

    public async Task<Message?> UpdateMessageAsync(MessageId messageId, string content, Uri[] attachments)
    {
        var message = await chatServiceRepository.FindMessageByIdAsync(messageId);

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
