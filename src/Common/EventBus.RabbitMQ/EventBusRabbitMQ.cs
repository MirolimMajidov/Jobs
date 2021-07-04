using Autofac;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : IEventBusRabbitMQ
    {
        /// <summary>
        /// Single name to use on RabbitMQ's exchange name and Autofac's scope name
        /// </summary>
        const string SingeAplicationName = "Microservices";

        private readonly ILogger<EventBusRabbitMQ> _logger;
        private readonly IEventBusSubscriptionsManager _subscriptionsManager;
        private readonly IRabbitMQConnection _connection;
        private readonly ILifetimeScope _autofac;

        private IModel _consumerChannel;
        private string _queueName;

        private bool canConnect => _connection.RetryCount > 0;

        public EventBusRabbitMQ(IRabbitMQConnection connection, IEventBusSubscriptionsManager subscriptionsManager, ILifetimeScope autofac, ILogger<EventBusRabbitMQ> logger, string queueName)
        {
            _subscriptionsManager = subscriptionsManager;
            _connection = connection;
            _autofac = autofac;
            _logger = logger;
            _queueName = queueName;
            _consumerChannel = canConnect ? CreateConsumerChannel() : null;

            _subscriptionsManager.OnEventRemoved += SubscriptionsManager_OnEventRemoved;
        }

        /// <summary/>
        public void Publish(RabbitMQEvent @event)
        {
            if (!canConnect) return;

            OpenRabbitMQConnectionIfItIsNotOpened();

            var policy = Policy.Handle<BrokerUnreachableException>().Or<SocketException>()
                .WaitAndRetry(_connection.RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
                });

            var eventName = @event.GetType().Name;
            _logger.LogTrace("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", @event.Id, eventName);

            using var channel = _connection.CreateModel();
            channel.ExchangeDeclare(exchange: SingeAplicationName, type: ExchangeType.Direct);

            var message = JsonConvert.SerializeObject(@event);
            var messageBody = Encoding.UTF8.GetBytes(message);

            policy.Execute(() =>
            {
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent

                channel.BasicPublish(
                    exchange: SingeAplicationName,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: messageBody);
            });
        }

        /// <summary/>
        public void Subscribe<TEvent, TEventHandler>()
            where TEvent : RabbitMQEvent
            where TEventHandler : IRabbitMQEventHandler<TEvent>
        {
            if (!canConnect) return;

            var eventName = _subscriptionsManager.GetEventKey<TEvent>();
            if (!_subscriptionsManager.HasSubscription(eventName))
            {
                OpenRabbitMQConnectionIfItIsNotOpened();
                using var channel = _connection.CreateModel();
                channel.QueueBind(queue: _queueName, exchange: SingeAplicationName, routingKey: eventName);
            }

            _logger.LogTrace("Subscribing {EventName} with {EventHandler}", eventName, typeof(TEventHandler).GetGenericTypeName());

            _subscriptionsManager.AddSubscription<TEvent, TEventHandler>();

            StartAndSubscribeReceiver();
        }

        /// <summary/>
        public void Unsubscribe<TEvent, TEventHandler>()
            where TEvent : RabbitMQEvent
            where TEventHandler : IRabbitMQEventHandler<TEvent>
        {
            if (!canConnect) return;

            var eventName = _subscriptionsManager.GetEventKey<TEvent>();
            _logger.LogTrace("Unsubscribing from event {EventName}", eventName);

            _subscriptionsManager.RemoveSubscription<TEvent, TEventHandler>();
        }

        /// <summary>
        /// To create channel for consumer
        /// </summary>
        /// <returns>Returns create channel</returns>
        private IModel CreateConsumerChannel()
        {
            OpenRabbitMQConnectionIfItIsNotOpened();

            if (!_connection.IsConnected) return null;

            _logger.LogTrace("Creating RabbitMQ consumer channel");

            var channel = _connection.CreateModel();

            channel.ExchangeDeclare(exchange: SingeAplicationName, type: ExchangeType.Direct);

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartAndSubscribeReceiver();
            };

            return channel;
        }

        /// <summary>
        /// To subscribe Received event for consumer
        /// </summary>
        private void StartAndSubscribeReceiver()
        {
            if (_consumerChannel == null)
            {
                _logger.LogError("StartBasicConsume can't call when _consumerChannel == null");
            }
            else
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
            }
        }

        /// <summary>
        /// An event to receive all sended events
        /// </summary>
        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            try
            {
                if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                    throw new InvalidOperationException($"Fake exception requested: \"{message}\"");

                _logger.LogTrace("Received RabbitMQ event: {EventName}", eventName);

                if (_subscriptionsManager.HasSubscription(eventName))
                {
                    using var scope = _autofac.BeginLifetimeScope(SingeAplicationName);
                    var subscription = _subscriptionsManager.GetEventHandler(eventName);
                    var handler = scope.ResolveOptional(subscription);
                    if (handler == null) return;

                    var eventType = _subscriptionsManager.GetEventType(eventName);
                    var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                    var concreteType = typeof(IRabbitMQEventHandler<>).MakeGenericType(eventType);

                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                }
                else
                {
                    _logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
            }

            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        /// <summary>
        /// To unbind removed event
        /// </summary>
        /// <param name="sender">Subscriptions manager</param>
        /// <param name="eventName">Removed event name</param>
        private void SubscriptionsManager_OnEventRemoved(object sender, string eventName)
        {
            OpenRabbitMQConnectionIfItIsNotOpened();

            using var channel = _connection.CreateModel();
            channel.QueueUnbind(queue: _queueName, exchange: SingeAplicationName, routingKey: eventName);

            if (_subscriptionsManager.IsEmpty)
            {
                _queueName = string.Empty;
                _consumerChannel.Close();
            }
        }

        /// <summary>
        /// To open connection if it's not opened
        /// </summary>
        void OpenRabbitMQConnectionIfItIsNotOpened()
        {
            if (!_connection.IsConnected)
            {
                try
                {
                    _connection.TryConnect();
                }
                catch (Exception)
                { }
            }
        }

        /// <summary>
        /// To close opened connection and clear all subscriptions from manager before disposing
        /// </summary>
        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }

            _subscriptionsManager.Clear();
        }
    }
}
