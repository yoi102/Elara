using DomainCommons;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Identity;
using Strongly;
using System.Diagnostics.CodeAnalysis;

namespace SocialLink.Domain.Entities
{


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
        public DateTimeOffset CreationTime { get; private set; }
        public DateTimeOffset? DeletionTime { get; private set; }
        public string? DisplayName { get; set; }
        public bool IsDeleted { get; private set; }
        public ICollection<WorkspaceId> WorkspaceIds { get; private set; } = new HashSet<WorkspaceId>();

        public void SoftDelete()
        {
            IsDeleted = true;
            DeletionTime = DateTimeOffset.Now;
        }
    }
}