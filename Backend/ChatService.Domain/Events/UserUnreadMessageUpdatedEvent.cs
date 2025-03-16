using ChatService.Domain.Entities;
using MediatR;

namespace ChatService.Domain.Events;

public record UserUnreadMessageUpdatedEvent(UserUnreadMessage Value) : INotification;
