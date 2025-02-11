using ChatService.Domain.Entities;
using MediatR;

namespace ChatService.Domain.Events;
public record ParticipantCreatedEvent(Participant Value) : INotification;
