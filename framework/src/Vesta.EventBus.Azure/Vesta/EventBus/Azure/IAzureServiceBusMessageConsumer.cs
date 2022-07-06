using Azure.Messaging.ServiceBus;

namespace Vesta.EventBus.Azure
{
    public interface IAzureServiceBusMessageConsumer
    {
        void Initialize(string topicName, string subscriberName, string connectionString);
        void OnMessageReceived(Func<ServiceBusReceivedMessage, Task> processEventAsync);
    }
}