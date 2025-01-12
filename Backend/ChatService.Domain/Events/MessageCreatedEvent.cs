﻿using ChatService.Domain.Entities;
using MediatR;

namespace ChatService.Domain.Events;

public record MessageCreatedEvent(BaseMessage Value) : INotification;
