using ChatService.Domain.Entities;
using MediatR;

namespace ChatService.Domain.Events;

public record MessageAttachmentCreatedEvent(MessageAttachment Value) : INotification;
