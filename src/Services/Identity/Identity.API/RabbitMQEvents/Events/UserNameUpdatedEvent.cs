using EventBus.RabbitMQ;
using System;

namespace IdentityService.RabbitMQEvents.Events
{
    public class UserNameUpdatedEvent : RabbitMQEvent
    {
        public Guid UserId { get; set; }
        public string OldName { get; set; }
        public string NewName { get; set; }
    }
}