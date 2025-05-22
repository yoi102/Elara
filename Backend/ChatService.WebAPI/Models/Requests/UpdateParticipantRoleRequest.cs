using DomainCommons.EntityStronglyIds;

namespace ChatService.WebAPI.Models.Requests;

public record UpdateParticipantRoleRequest(ParticipantId Id, string Role);

