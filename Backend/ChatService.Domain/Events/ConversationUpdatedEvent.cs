using ChatService.Domain.Entities;
using MediatR;

namespace ChatService.Domain.Events;

public record ConversationUpdatedEvent(Conversation Value) : INotification;
