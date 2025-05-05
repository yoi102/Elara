namespace ApiClients.Abstractions.ChatApiClient.Participant.Requests;

public record UpdateParticipantRoleRequest(Guid Id, string Role);
