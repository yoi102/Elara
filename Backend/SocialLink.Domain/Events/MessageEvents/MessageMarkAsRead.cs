using MediatR;
using SocialLink.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLink.Domain.Events.MessageEvents
{
    public record class MessageMarkAsRead(Message Message) : INotification;

}
