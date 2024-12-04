using DomainCommons.EntityStronglyIds;
using PersonalSpaceService.Domain.Entities;

namespace PersonalSpaceService.Domain.Interfaces
{
    public interface IPersonalSpaceRepository
    {

        Task<Contact> AddContactAsync(UserId userId, string remark);
        Task<Profile> CreateProfileAsync(UserId userId, string displayName);



    }
}
