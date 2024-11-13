using SocialLink.Domain.Entities;

namespace SocialLink.Domain.Interfaces
{
    public interface IConversationRepository
    {
        Task<Message[]> GetConversationAllMessagesAsync(ConversationId id);

        Task<Participant[]> GetConversationAllParticipantsAsync(ConversationId id);

        Task<Conversation?> FindByIdAsync(ConversationId id);
    }
}