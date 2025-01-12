using ChatService.Domain.Entities;
using MediatR;

namespace ChatService.Domain.Events;
public record GroupConversationMemberUpdatedEvent(GroupConversationMember Value) : INotification;

