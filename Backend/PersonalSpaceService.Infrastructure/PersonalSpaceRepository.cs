using DomainCommons.EntityStronglyIds;
using Microsoft.EntityFrameworkCore;
using PersonalSpaceService.Domain.Entities;
using PersonalSpaceService.Domain.Interfaces;

namespace PersonalSpaceService.Infrastructure;

internal class PersonalSpaceRepository : IPersonalSpaceRepository
{
    private readonly PersonalSpaceDbContext dbContext;

    public PersonalSpaceRepository(PersonalSpaceDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    #region Contact

    public async Task<Contact> AddContactAsync(UserId ownerId, UserId contactId, string remark)
    {
        var contact = new Contact(ownerId, contactId, remark);
        var entityEntry = await dbContext.Contacts.AddAsync(contact);
        return entityEntry.Entity;
    }

    public async Task<Contact[]> AllUserContactsAsync(UserId userId)
    {
        return await dbContext.Contacts.Where(c => c.OwnerId == userId).ToArrayAsync();
    }

    public async Task<Task> DeleteContactAsync(ContactId contactId)
    {
        var contact = await dbContext.Contacts.FindAsync(contactId);
        if (contact == null)
            return Task.CompletedTask;
        dbContext.Contacts.Remove(contact);
        return Task.CompletedTask;
    }

    public async Task<Contact?> FindContactByContactIdAsync(ContactId contactId)
    {
        return await dbContext.Contacts.FindAsync(contactId);
    }

    public async Task<Contact?> UpdateContactInfoAsync(ContactId contactId, string remark)
    {
        var contact = await dbContext.Contacts.FindAsync(contactId);
        if (contact == null)
            return null;
        contact.ChangeRemark(remark);
        return contact;
    }

    #endregion Contact

    #region Profile

    public async Task<Profile> CreateProfileAsync(UserId userId, string displayName)
    {
        var profile = new Profile(userId, displayName);
        var entityEntry = await dbContext.Profiles.AddAsync(profile);
        return entityEntry.Entity;
    }

    public async Task<Profile?> DeleteProfileByUserIdAsync(UserId userId)
    {
        var profile = await FindProfileByUserIdAsync(userId);
        if (profile == null)
        {
            return profile;
        }
        profile.SoftDelete();
        return profile;
    }

    public async Task<Profile?> FindProfileByProfileIdAsync(ProfileId profileId)
    {
        return await dbContext.Profiles.FindAsync(profileId);
    }

    public async Task<Profile?> FindProfileByUserIdAsync(UserId userId)
    {
        return await dbContext.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<Profile?> UpdateProfileAsync(ProfileId profileId, string displayName, Uri avatar)
    {
        var profile = await dbContext.Profiles.FindAsync(profileId);
        if (profile == null)
            return null;
        profile.ChangeDisplayName(displayName);
        profile.ChangeAvatar(avatar);
        return profile;
    }

    #endregion Profile
}