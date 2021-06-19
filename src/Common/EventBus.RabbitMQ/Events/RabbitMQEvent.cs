using Newtonsoft.Json;
using System;

namespace EventBus.RabbitMQ
{
    /// <summary>
    /// Base class for all RabbitMQ event classes 
    /// </summary>
    public abstract class RabbitMQEvent
    {
        public RabbitMQEvent()
        {
            Id = Guid.NewGuid();
            CreatedTime = DateTime.UtcNow;
        }

        [JsonConstructor]
        public RabbitMQEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreatedTime = createDate;
        }

        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public DateTime CreatedTime { get; private set; }
    }
}
