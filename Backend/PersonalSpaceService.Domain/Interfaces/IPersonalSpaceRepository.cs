using DomainCommons.EntityStronglyIds;
using PersonalSpaceService.Domain.Entities;

namespace PersonalSpaceService.Domain.Interfaces
{
    public interface IPersonalSpaceRepository
    {
        #region Contact

        Task<Contact> AddContactAsync(UserId ownerId, UserId contactId, string remark);

        Task<Contact[]> AllUserContactsAsync(UserId userId);

        Task<Task> DeleteContactAsync(ContactId contactId);

        Task<Contact?> FindContactByContactIdAsync(ContactId contactId);

        Task<Contact?> UpdateContactInfoAsync(ContactId contactId, string remark);

        #endregion Contact

        #region Profile

        Task<Profile> CreateProfileAsync(UserId userId, string displayName);

        Task<Profile?> FindProfileByProfileIdAsync(ProfileId profileId);

        Task<Profile?> FindProfileByUserIdAsync(UserId userId);

        Task<Profile?> UpdateProfileAsync(ProfileId profileId, string displayName, Uri avatar);

        #endregion Profile
    }
}