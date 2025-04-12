using ChatService.Domain.Entities;
using MediatR;

namespace ChatService.Domain.Events;

public record ConversationRequestCreatedEvent(ConversationRequest Value) : INotification;
