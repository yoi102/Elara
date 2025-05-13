using ChatService.Domain.Entities;
using MediatR;

namespace ChatService.Domain.Events;

public record UserUnreadConversationRequestCreatedEvent(UserUnreadConversationRequest Value) : INotification;
