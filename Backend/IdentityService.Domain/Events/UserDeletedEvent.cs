using IdentityService.Domain.Entities;
using MediatR;

namespace IdentityService.Domain.Events;

public record UserDeletedEvent(User User) : INotification;