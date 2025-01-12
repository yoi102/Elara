using ChatService.Domain.Entities;
using MediatR;

namespace ChatService.Domain.Events;
public record GroupConversationMemberCreatedEvent(GroupConversationMember Value) : INotification;
