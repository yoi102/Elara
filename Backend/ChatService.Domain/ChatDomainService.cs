using ChatService.Domain.Entities;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain;

public class ChatDomainService
{
    //这里应该提供 new 实体、实体类应该不允许此项目外 new 的

    private readonly IChatServiceRepository chatServiceRepository;

    public ChatDomainService(IChatServiceRepository chatServiceRepository)
    {
        this.chatServiceRepository = chatServiceRepository;
    }

    public async Task<Conversation?> ChangeConversationNameAsync(ConversationId conversationId, string name)
    {
        var conversation = await chatServiceRepository.FindConversationByIdAsync(conversationId);
        conversation?.ChangeName(name);

        return conversation;
    }
}
