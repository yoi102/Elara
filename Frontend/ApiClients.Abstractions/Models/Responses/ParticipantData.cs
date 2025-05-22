
namespace ApiClients.Abstractions.Models.Responses;


public record ParticipantData
{
    public required Guid Id { get; set; }
    public required Guid ConversationId { get; set; }
    public required string Role { get; set; }
    public required UserInfoData UserInfo { get; set; }
}
