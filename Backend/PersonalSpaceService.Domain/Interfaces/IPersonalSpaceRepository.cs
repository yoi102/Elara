using DomainCommons.EntityStronglyIds;
using DomainCommons.Enums;
using PersonalSpaceService.Domain.Entities;

namespace PersonalSpaceService.Domain.Interfaces;

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

    Task<Profile?> DeleteProfileByUserIdAsync(UserId userId);

    Task<Profile?> FindProfileByProfileIdAsync(ProfileId profileId);

    Task<Profile?> FindProfileByUserIdAsync(UserId userId);

    Task<Profile?> UpdateProfileAsync(ProfileId profileId, string displayName, UploadedItemId avatar);

    #endregion Profile

    #region ContactRequest

    Task<ContactRequest[]> AllContactRequestByReceiverIdAsync(UserId receiverId);

    Task<ContactRequest> CreateContactRequestAsync(UserId senderId, UserId receiverId);

    Task<ContactRequest?> FindContactRequestByIdAsync(ContactRequestId contactRequestId);

    Task<ContactRequest[]> GetPendingContactRequestByReceiverIdAsync(UserId receiverId);

    Task<ContactRequest?> UpdateContactRequestAsync(ContactRequestId contactRequestId, RequestStatus status);

    #endregion ContactRequest
}
