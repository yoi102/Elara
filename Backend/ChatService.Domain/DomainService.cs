using ChatService.Domain.Entities;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain
{
    public class DomainService
    {
        private readonly IChatServiceRepository chatServiceRepository;

        public DomainService(IChatServiceRepository chatServiceRepository)
        {
            this.chatServiceRepository = chatServiceRepository;
        }

        public async Task<GroupConversation?> ChangeGroupConversationName(GroupConversationId groupConversationId, string name)
        {
            var groupConversation = await chatServiceRepository.FindGroupConversationByIdAsync(groupConversationId);
            if (groupConversation is not null)
            {
                groupConversation.ChangeName(name);
            }

            return groupConversation;
        }


        public async Task<MessageBase?> UpdateMessage(MessageId messageId, string content, Uri[] attachments)
        {
            MessageBase? message = await chatServiceRepository.FindGroupMessagesByIdAsync(messageId);
            message ??= await chatServiceRepository.FindPersonalMessagesByIdAsync(messageId);
            if (message is null)
            {
                return null;
            }
            message.ChangeContent(content);
            message.ChangeAttachments(attachments);
            return message;
        }










    }
}
