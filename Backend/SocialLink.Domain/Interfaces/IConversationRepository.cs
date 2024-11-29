using SocialLink.Domain.Entities;

namespace SocialLink.Domain.Interfaces
{
    public interface IConversationRepository
    {
        Task<Message[]> GetConversationAllMessagesAsync(PersonalConversationId id);

        Task<Participant[]> GetConversationAllParticipantsAsync(PersonalConversationId id);

        Task<PersonalConversation?> FindByIdAsync(PersonalConversationId id);
    }
}