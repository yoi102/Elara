using IdentityService.Domain.Entities;
using MediatR;

namespace IdentityService.Domain.Events;

public record UserCreatedEvent(User User) : INotification;