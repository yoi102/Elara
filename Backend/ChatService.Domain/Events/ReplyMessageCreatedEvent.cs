using ChatService.Domain.Entities;
using MediatR;

namespace ChatService.Domain.Events;

public record ReplyMessageCreatedEvent(ReplyMessage Value) : INotification;
