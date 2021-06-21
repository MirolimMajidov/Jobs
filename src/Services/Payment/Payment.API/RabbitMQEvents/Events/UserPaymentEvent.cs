using EventBus.RabbitMQ;
using System;

namespace PaymentService.RabbitMQEvents.Events
{
    public class UserPaymentEvent : RabbitMQEvent
    {
        public Guid UserId { get; set; }
        public double NewBalance { get; set; }
    }
}