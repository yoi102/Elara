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











    }
}
