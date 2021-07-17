using EventBus.RabbitMQ;
using System;

namespace IdentityService.RabbitMQEvents.Events
{
    public class UserTransactionEvent : RabbitMQEvent
    {
        public Guid UserId { get; set; }
        public double Amount { get; set; }
    }
}