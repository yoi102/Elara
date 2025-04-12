using MediatR;
using PersonalSpaceService.Domain.Entities;

namespace PersonalSpaceService.Domain.Events;

public record ContactRequestUpdatedEvent(ContactRequest Value) : INotification;
