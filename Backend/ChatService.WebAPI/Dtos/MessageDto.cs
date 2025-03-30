using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Dtos;
public record MessageDto
{
    public MessageId Id { get; set; }

    public required UserDto Sender { get; set; }

    public required string Message { get; set; }

    public MessageDto? QuoteMessage { get; set; }

    public required ReplyMessageDto[] ReplyMessage1s { get; set; }

    public required Uri[] Attachments { get; set; }
}
