using ChatService.Domain.Entities;
using MediatR;

namespace ChatService.Domain.Events;

public record MessageUpdatedEvent(BaseMessage Value) : INotification;
