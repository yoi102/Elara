using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Handlers
{
    public abstract class IntegrationEventHandler : IIntegrationEventHandler
    {
        public abstract Task Handle(string eventName, string eventData);
    }
}