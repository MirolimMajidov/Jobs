using System;
using System.Collections.Generic;
using System.Linq;

namespace EventBus.RabbitMQ
{
    public partial class EventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        /// <summary>
        /// Dictionary collection to store all event and event handler informations
        /// </summary>
        private readonly Dictionary<string, (Type EventType, Type EventHandlerType)> _subscriptions;

        /// <summary/>
        public event EventHandler<string> OnEventRemoved;

        public EventBusSubscriptionsManager()
        {
            _subscriptions = new Dictionary<string, (Type EventType, Type EventHandlerType)>();
        }

        /// <summary/>
        public bool IsEmpty => !_subscriptions.Any();

        /// <summary/>
        public void AddSubscription<TEvent, TEventHandler>()
            where TEvent : RabbitMQEvent
            where TEventHandler : IRabbitMQEventHandler<TEvent>
        {
            var eventType = typeof(TEvent);
            var handlerType = typeof(TEventHandler);

            if (HasSubscription(eventType.Name))
                throw new ArgumentException($"{handlerType.Name} handler type already registered for '{eventType.Name}'", nameof(handlerType));
            else
                _subscriptions.Add(eventType.Name, (eventType, handlerType));
        }

        /// <summary/>
        public void RemoveSubscription<TEvent, TEventHandler>()
            where TEvent : RabbitMQEvent
            where TEventHandler : IRabbitMQEventHandler<TEvent>
        {
            var eventName = GetEventKey<TEvent>();
            if (HasSubscription(eventName))
            {
                _subscriptions.Remove(eventName);

                RaiseOnEventRemoved(eventName);
            }
        }

        /// <summary/>
        public bool HasSubscription(string eventName) => _subscriptions.ContainsKey(eventName);

        /// <summary/>
        public Type GetEventHandler(string eventName) => _subscriptions[eventName].EventHandlerType;

        /// <summary>
        /// The method to fire subscribed event after removing event
        /// </summary>
        /// <param name="eventName">Removed event name</param>
        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            handler?.Invoke(this, eventName);
        }

        /// <summary/>
        public Type GetEventType(string eventName) => _subscriptions[eventName].EventType;

        /// <summary/>
        public string GetEventKey<TEvent>()
        {
            return typeof(TEvent).Name;
        }

        /// <summary/>
        public void Clear()
        {
            _subscriptions.Clear();
        }
    }
}
