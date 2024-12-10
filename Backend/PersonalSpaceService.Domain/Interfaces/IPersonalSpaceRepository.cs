using DomainCommons.EntityStronglyIds;
using PersonalSpaceService.Domain.Entities;

namespace PersonalSpaceService.Domain.Interfaces
{
    public interface IPersonalSpaceRepository
    {
        #region Contact

        Task<Contact> AddContactAsync(UserId userId, string remark);

        Task<Contact> AllUserContactAsync(UserId userId);

        Task<Contact> DeleteContactAsync(ContactId contactId);

        Task<Contact> GetContactAsync(ContactId contactId);

        Task<Contact> UpdateContactInfoAsync(ContactId contactId, string remark);

        #endregion Contact

        #region Profile

        Task<Profile> CreateProfileAsync(UserId userId, string displayName);

        Task<Profile> UpdateProfileAsync(UserId userId, string displayName, Uri avatar);

        #endregion Profile
    }
}