using DomainCommons;
using Microsoft.AspNetCore.Identity;
using Strongly;
using System.Diagnostics.CodeAnalysis;

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
            Workspaces = new HashSet<WorkspaceId>();
        }
        private User()
        {
        }

        public Uri? Avatar { get; set; }
        public DateTimeOffset CreationTime { get; private set; }
        public DateTimeOffset? DeletionTime { get; private set; }
        public string? DisplayName { get; set; }
        public bool IsDeleted { get; private set; }
        [NotNull]
        public ICollection<WorkspaceId> Workspaces { get; private set; }

        public void SoftDelete()
        {
            IsDeleted = true;
            DeletionTime = DateTimeOffset.Now;
        }
    }
}