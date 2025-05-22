using DomainCommons.EntityStronglyIds;

namespace ChatService.WebAPI.Models.Responses;

public record ConversationInfoResponse
{
    public required ConversationId Id { get; set; }
    public required string Name { get; set; }
    public required bool IsGroup { get; set; }
    //public required string? Avatar { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
}

