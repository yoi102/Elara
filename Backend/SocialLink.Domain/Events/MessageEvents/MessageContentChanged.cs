using MediatR;
using SocialLink.Domain.Entities;

namespace SocialLink.Domain.Events.MessageEvents
{

    public record class MessageContentChanged(Message Message) : INotification;

}
