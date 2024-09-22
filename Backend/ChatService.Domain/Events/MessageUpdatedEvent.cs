using ChatService.Domain.Entities;
using MediatR;

namespace ChatService.Domain.Events;

public record MessageUpdatedEvent(Message Value) : INotification;
