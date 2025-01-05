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


        //public async Task<MessageBase> UpdateMessage(MessageId messageId, string content)
        //{



        //}










    }
}
