using RabbitMQ.Client;
using System;

namespace EventBus.RabbitMQ
{
    public interface IRabbitMQConnection : IDisposable
    {
        /// <summary>
        /// Returns true, when RabbitMQ connected
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// To get retry count
        /// </summary>
        int RetryCount { get;  }

        /// <summary>
        /// For reconnection connection when connection is disconnected
        /// </summary>
        /// <returns>Returns true, if it's successful connected</returns>
        bool TryConnect();

        /// <summary>
        /// To create a model after openning connection
        /// </summary>
        /// <returns>Returns created model</returns>
        IModel CreateModel();
    }
}
