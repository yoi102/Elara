﻿using ChatService.Domain.Entities;
using MediatR;

namespace ChatService.Domain.Events;

public record ConversationRequestUpdatedEvent(ConversationRequest Value) : INotification;
