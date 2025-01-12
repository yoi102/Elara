using ChatService.Domain.Entities;
using MediatR;

namespace ChatService.Domain.Events;

public record GroupConversationUpdatedEvent(GroupConversation Value) : INotification;
