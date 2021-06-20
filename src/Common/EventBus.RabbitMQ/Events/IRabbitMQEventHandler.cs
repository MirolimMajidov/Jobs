using System.Threading.Tasks;

namespace EventBus.RabbitMQ
{
    /// <summary>
    /// Base interface for all RabbitMQ event handler classes
    /// </summary>
    public interface IRabbitMQEventHandler<TRabbitMQEvent> where TRabbitMQEvent : RabbitMQEvent
    {
        /// <summary>
        /// To receive a message 
        /// </summary>
        /// <param name="event">Send event by RabbitMQ</param>
        Task Handle(TRabbitMQEvent @event);
    }
}
