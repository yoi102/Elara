using DomainCommons.EntityStronglyIds;
using PersonalSpaceService.Domain.Entities;
using PersonalSpaceService.Domain.Interfaces;

namespace PersonalSpaceService.Domain
{
    public class PersonalSpaceDomainService
    {
        private readonly IPersonalSpaceRepository personalSpaceRepository;

        public PersonalSpaceDomainService(IPersonalSpaceRepository personalSpaceRepository)
        {
            this.personalSpaceRepository = personalSpaceRepository;
        }


    













    }
}
