using DomainCommons.EntityStronglyIds;

namespace ChatService.WebAPI.Controllers.ParticipantController.Requests;

public record UpdateParticipantRoleRequest(ParticipantId Id, string Role);

