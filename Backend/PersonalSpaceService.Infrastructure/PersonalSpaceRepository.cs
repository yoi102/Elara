using DomainCommons.EntityStronglyIds;
using PersonalSpaceService.Domain.Entities;
using PersonalSpaceService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSpaceService.Infrastructure
{
    internal class PersonalSpaceRepository : IPersonalSpaceRepository
    {
        private readonly PersonalSpaceDbContext personalSpaceDbContext;

        public PersonalSpaceRepository(PersonalSpaceDbContext personalSpaceDbContext)
        {
            this.personalSpaceDbContext = personalSpaceDbContext;
        }

        public async Task<Contact> AddContactAsync(UserId userId, string remark)
        {
            var contact = new Contact(userId, remark);
            var entityEntry = await personalSpaceDbContext.Contacts.AddAsync(contact);
            return entityEntry.Entity;
        }

        public async Task<Profile> CreateProfileAsync(UserId userId, string displayName)
        {
            var profile = new Profile(userId, displayName);
            var entityEntry = await personalSpaceDbContext.Profiles.AddAsync(profile);
            return entityEntry.Entity;
        }
    }
}