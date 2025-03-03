using MediatR;
using PersonalSpaceService.Domain.Entities;

namespace PersonalSpaceService.Domain.Events;

public record ContactRequestUpdateEvent(ContactRequest Value) : INotification;
