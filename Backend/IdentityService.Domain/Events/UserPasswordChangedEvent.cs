using IdentityService.Domain.Entities;
using MediatR;

namespace IdentityService.Domain.Events;

public record UserPasswordChangedEvent(User User) : INotification;