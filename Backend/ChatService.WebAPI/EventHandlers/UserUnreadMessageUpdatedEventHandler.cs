﻿using ChatService.Domain.Events;
using MediatR;

namespace ChatService.WebAPI.EventHandlers;

public class UserUnreadMessageUpdatedEventHandler : INotificationHandler<UserUnreadMessageUpdatedEvent>
{
    public Task Handle(UserUnreadMessageUpdatedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
