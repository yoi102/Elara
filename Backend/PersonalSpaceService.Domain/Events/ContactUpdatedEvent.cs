using MediatR;
using PersonalSpaceService.Domain.Entities;

namespace PersonalSpaceService.Domain.Events;

public record ContactUpdatedEvent(Contact Value) : INotification;
