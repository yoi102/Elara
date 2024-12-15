using DomainCommons.EntityStronglyIds;
using Grpc.Core;
using Personal;
using PersonalSpaceService.Domain.Interfaces;

namespace PersonalSpaceService.WebAPI.Services
{
    public class PersonService : Person.PersonBase
    {
        private readonly IPersonalSpaceRepository repository;

        public PersonService(IPersonalSpaceRepository repository)
        {
            this.repository = repository;
        }

        public override async Task<CreateProfileReply> CreateProfile(CreateProfileRequest request, ServerCallContext context)
        {
            var result = new CreateProfileReply() { IsCreated = false };
            if (!UserId.TryParse(request.UserId, out var userId))
            {
                return result;
            }
            if (string.IsNullOrEmpty(request.UserName))
            {
                return result;
            }

            await repository.CreateProfileAsync(userId, request.UserName);
            result.IsCreated = true;
            return result;
        }
    }
}