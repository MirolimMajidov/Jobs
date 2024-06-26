namespace EventBus.RabbitMQ
{
    public class RabbitMQConfigurationInfo
    {
        /// <summary>
        /// Connection string to the host of RabbitMQ
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Subscription/Queue name
        /// </summary>
        public string SubscriptionName { get; set; }

        /// <summary>
        /// Retry count to publish event
        /// </summary>
        public int RetryPublishCount { get; set; }
    }
}
