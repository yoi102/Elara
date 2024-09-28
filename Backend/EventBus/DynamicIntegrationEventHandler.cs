﻿using Dynamic.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EventBus
{
    public abstract class DynamicIntegrationEventHandler : IIntegrationEventHandler
    {
        public Task Handle(string eventName, string eventData)
        {
            //https://github.com/dotnet/runtime/issues/53195
            //https://github.com/dotnet/core/issues/644
            dynamic dynamicEventData = DJson.Parse(eventData);
            return HandleDynamic(eventName, dynamicEventData);
        }

        public abstract Task HandleDynamic(string eventName, dynamic eventData);
    }
}