using ChatService.Domain.Entities;
using MediatR;

namespace ChatService.Domain.Events;

public record UserUnreadMessageCreatedEvent(UserUnreadMessage Value) : INotification;
