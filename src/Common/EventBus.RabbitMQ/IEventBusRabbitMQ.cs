namespace EventBus.RabbitMQ
{
    public interface IEventBusRabbitMQ
    {
        /// <summary>
        /// For publishing created event to all subscribed event handlers 
        /// </summary>
        /// <param name="event">Created event for publish</param>
        void Publish(RabbitMQEvent @event);

        /// <summary>
        /// To add subscription to the subscriptions manager
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <typeparam name="TEventHandler">Event handler type</typeparam>
        void Subscribe<TEvent, TEventHandler>()
            where TEvent : RabbitMQEvent
            where TEventHandler : IRabbitMQEventHandler<TEvent>;

        /// <summary>
        /// To remove subscription from the subscriptions manager
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <typeparam name="TEventHandler">Event handler type</typeparam>
        void Unsubscribe<TEvent, TEventHandler>()
            where TEvent : RabbitMQEvent
            where TEventHandler : IRabbitMQEventHandler<TEvent>;
    }
}
