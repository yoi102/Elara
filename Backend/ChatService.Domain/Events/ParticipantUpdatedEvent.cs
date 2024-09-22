using ChatService.Domain.Entities;
using MediatR;

namespace ChatService.Domain.Events;
public record ParticipantUpdatedEvent(Participant Value) : INotification;

