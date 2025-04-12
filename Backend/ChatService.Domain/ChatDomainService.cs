using ChatService.Domain.Entities;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain;

public class ChatDomainService
{
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
