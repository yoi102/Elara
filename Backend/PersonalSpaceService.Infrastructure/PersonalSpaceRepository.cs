using DomainCommons.EntityStronglyIds;
using PersonalSpaceService.Domain.Entities;
using PersonalSpaceService.Domain.Interfaces;

namespace PersonalSpaceService.Infrastructure
{
    internal class PersonalSpaceRepository : IPersonalSpaceRepository
    {
        private readonly PersonalSpaceDbContext dbContext;

        public PersonalSpaceRepository(PersonalSpaceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #region Contact

        public async Task<Contact> AddContactAsync(UserId userId, string remark)
        {
            var contact = new Contact(userId, remark);
            var entityEntry = await dbContext.Contacts.AddAsync(contact);
            return entityEntry.Entity;
        }

        public Task<Contact> AllUserContactAsync(UserId userId)
        {
            throw new NotImplementedException();
        }

        public Task<Contact> DeleteContactAsync(ContactId contactId)
        {
            throw new NotImplementedException();
        }

        public Task<Contact> GetContactAsync(ContactId contactId)
        {
            throw new NotImplementedException();
        }

        public Task<Contact> UpdateContactInfoAsync(ContactId contactId, string remark)
        {
            throw new NotImplementedException();
        }

        #endregion Contact

        #region Profile

        public async Task<Profile> CreateProfileAsync(UserId userId, string displayName)
        {
            var profile = new Profile(userId, displayName);
            var entityEntry = await dbContext.Profiles.AddAsync(profile);
            return entityEntry.Entity;
        }

        public Task<Profile> UpdateProfileAsync(UserId userId, string displayName, Uri avatar)
        {
            throw new NotImplementedException();
        }

        #endregion Profile
    }
}