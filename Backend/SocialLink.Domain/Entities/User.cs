using DomainCommons;
using Microsoft.AspNetCore.Identity;
using Strongly;

namespace SocialLink.Domain.Entities
{
    [Strongly(converters: StronglyConverter.EfValueConverter |
                     StronglyConverter.SwaggerSchemaFilter |
                     StronglyConverter.SystemTextJson |
                     StronglyConverter.TypeConverter)]
    public partial struct UserId;

    public class User : IdentityUser<UserId>, ISoftDelete, IHasCreationTime, IHasDeletionTime
    {
        public User(string name, string email)
        {
            Id = UserId.New();
            Email = email;
            UserName = name;
            DisplayName = name;
            CreationTime = DateTimeOffset.Now;
        }

        private User()
        {
        }

        public Uri? Avatar { get; set; }
        public ICollection<ConversationId> ConversationIds { get; private set; } = new HashSet<ConversationId>();
        public DateTimeOffset CreationTime { get; private set; }
        public DateTimeOffset? DeletionTime { get; private set; }
        public string? DisplayName { get; set; }
        public bool IsDeleted { get; private set; }
        public ICollection<ContactInvitationId> ReceivedContactInvitationIds { get; private set; } = new HashSet<ContactInvitationId>();
        public ICollection<WorkspaceInvitationId> ReceivedWorkspaceInvitationIds { get; private set; } = new HashSet<WorkspaceInvitationId>();
        public ICollection<UserContactId> UserContactIds { get; private set; } = new HashSet<UserContactId>();
        public ICollection<WorkspaceId> WorkspaceIds { get; private set; } = new HashSet<WorkspaceId>();
        public void AddContact(UserContactId id)
        {
            UserContactIds.Add(id);
        }

        public void SoftDelete()
        {
            IsDeleted = true;
            DeletionTime = DateTimeOffset.Now;
        }
    }
}