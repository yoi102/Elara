using MediatR;
using PersonalSpaceService.Domain.Entities;

namespace PersonalSpaceService.Domain.Events;

public record ProfileUpdatedEvent(Profile Value) : INotification;
