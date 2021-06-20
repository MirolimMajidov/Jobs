using System;

namespace EventBus.RabbitMQ
{
    public interface IEventBusSubscriptionsManager
    {
        /// <summary>
        /// Returns true when manager doesn't have any subscriptions
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Event handler to bine and fire when any event removed
        /// </summary>
        event EventHandler<string> OnEventRemoved;

        /// <summary>
        /// To add subscription to the subscriptions manager
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <typeparam name="TEventHandler">Event handler type</typeparam>
        void AddSubscription<TEvent, TEventHandler>()
           where TEvent : RabbitMQEvent
           where TEventHandler : IRabbitMQEventHandler<TEvent>;

        /// <summary>
        /// To remove subscription from the subscriptions manager
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <typeparam name="TEventHandler">Event handler type</typeparam>
        void RemoveSubscription<TEvent, TEventHandler>()
           where TEvent : RabbitMQEvent
           where TEventHandler : IRabbitMQEventHandler<TEvent>;

        /// <summary>
        /// Returns true when subscriptions manager has any subscriptions with entered event name
        /// </summary>
        /// <param name="eventName">Event name to check subscription</param>
        /// <returns></returns>
        bool HasSubscription(string eventName);

        /// <summary>
        /// To get subscribed's EventHandler type by event name
        /// </summary>
        /// <param name="eventName">Event name</param>
        /// <returns>Event handler type</returns>
        public Type GetEventHandler(string eventName);

        /// <summary>
        /// To get subscribed's Event type by event name
        /// </summary>
        /// <param name="eventName">Event name</param>
        /// <returns>Event type</returns>
        public Type GetEventType(string eventName);

        /// <summary>
        /// To get event name by event type
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <returns>Event name</returns>
        string GetEventKey<T>();

        /// <summary>
        /// To clear all subscriptions
        /// </summary>
        void Clear();
    }
}