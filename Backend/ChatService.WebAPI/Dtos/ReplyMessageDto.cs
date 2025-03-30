using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Dtos;
public record ReplyMessageDto
{
    public MessageId Id { get; set; }

    public required UserDto Sender { get; set; }

    public required string Message { get; set; }

    public required Uri[] Attachments { get; set; }
}
