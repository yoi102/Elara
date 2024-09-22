using MediatR;
using PersonalSpaceService.Domain.Entities;

namespace PersonalSpaceService.Domain.Events;

public record ContactRequestCreatedEvent(ContactRequest Value) : INotification;
