using DomainCommons;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain.Entities
{
    public class User : IdentityUser<UserId>, ISoftDelete, IHasCreationTime, IHasDeletionTime
    {
        public User(string name, string email)
        {
            Id = UserId.New();
            Email = email;
            UserName = name;
            CreationTime = DateTimeOffset.Now;
        }

        private User()
        {
        }

        public DateTimeOffset CreationTime { get; private set; }
        public DateTimeOffset? DeletionTime { get; private set; }
        public bool IsDeleted { get; private set; }

        public void SoftDelete()
        {
            IsDeleted = true;
            DeletionTime = DateTimeOffset.Now;
        }
    }
}