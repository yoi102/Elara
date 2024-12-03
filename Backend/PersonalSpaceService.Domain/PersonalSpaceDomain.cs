using DomainCommons.EntityStronglyIds;
using PersonalSpaceService.Domain.Entities;
using PersonalSpaceService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSpaceService.Domain
{
    public class PersonalSpaceDomain
    {
        private readonly IPersonalSpaceRepository personalSpaceRepository;

        public PersonalSpaceDomain(IPersonalSpaceRepository personalSpaceRepository)
        {
            this.personalSpaceRepository = personalSpaceRepository;
        }


        public async Task<Contact> AddContactAsync(UserId userId, string remark)
        {
            var contact = new Contact(userId, remark);
            return contact;

        }












    }
}
